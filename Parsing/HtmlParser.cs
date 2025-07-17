using HtmlAgilityPack;
using System.Reflection;

namespace EClinicalWorksPoc.Parsing;

public interface IHtmlParser
{
    T Parse<T>(string htmlContent) where T : class, new();
}

public class HtmlParser : IHtmlParser
{
    public T Parse<T>(string htmlContent) where T : class, new()
    {
        var instance = new T();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<XPathSelectorAttribute>() != null);

        foreach (var property in properties)
        {
            var xpathAttribute = property.GetCustomAttribute<XPathSelectorAttribute>();
            if (xpathAttribute != null)
            {
                try
                {
                    var node = htmlDoc.DocumentNode.SelectSingleNode(xpathAttribute.XPath);
                    if (node != null)
                    {
                        string value;
                        
                        // Check if the XPath is selecting an attribute (ends with /@something)
                        if (xpathAttribute.XPath.Contains("/@"))
                        {
                            value = node.GetAttributeValue("content", string.Empty);
                        }
                        else
                        {
                            // For script tags or other elements, get the inner text or HTML
                            value = node.InnerText?.Trim() ?? node.InnerHtml?.Trim() ?? string.Empty;
                        }

                        property.SetValue(instance, value);
                    }
                }
                catch (Exception ex)
                {
                    // Log the error but continue processing other properties
                    // In a real application, you might want to use proper logging here
                    Console.WriteLine($"Error parsing XPath '{xpathAttribute.XPath}' for property '{property.Name}': {ex.Message}");
                }
            }
        }

        return instance;
    }
} 