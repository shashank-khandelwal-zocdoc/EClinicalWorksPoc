namespace EClinicalWorksPoc.Parsing;

[AttributeUsage(AttributeTargets.Property)]
public class XPathSelectorAttribute(string xpath) : Attribute
{
    public string XPath { get; } = xpath;
} 