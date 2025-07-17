using System.Net;
using EClinicalWorksPoc.Logging;

namespace EClinicalWorksPoc.Http;

public class CookieManager(ICustomLogger logger)
{
    private readonly CookieCollection _cookies = new();

    /// <summary>
    /// Gets the current cookie collection
    /// </summary>
    public CookieCollection Cookies => _cookies;

    /// <summary>
    /// Extracts and persists cookies from an HTTP response
    /// </summary>
    public void ExtractAndPersistCookies(HttpResponseMessage response, string requestUrl)
    {
        try
        {
            // Extract Set-Cookie headers from the response
            if (response.Headers.TryGetValues("Set-Cookie", out var setCookieHeaders))
            {
                var uri = new Uri(requestUrl);
                
                foreach (var setCookieHeader in setCookieHeaders)
                {
                    // Parse each Set-Cookie header and add to our collection
                    var cookie = ParseSetCookieHeader(setCookieHeader, uri);
                    if (cookie != null)
                    {
                        // Update existing cookie or add new one
                        UpdateOrAddCookie(cookie);
                        
                        logger.Information("Extracted cookie: {CookieName}={CookieValue} (Domain: {Domain}, Path: {Path})",
                            cookie.Name, cookie.Value, cookie.Domain, cookie.Path);
                    }
                }

                logger.Information("Total cookies persisted: {CookieCount}", _cookies.Count);
            }
            else
            {
                logger.Information("No cookies found in response headers");
            }
        }
        catch (Exception ex)
        {
            logger.Information("Failed to extract cookies from response: {Error}", ex.Message);
        }
    }

    /// <summary>
    /// Gets cookies as a string suitable for Cookie header
    /// </summary>
    public string GetCookieHeaderValue(string url)
    {
        var uri = new Uri(url);
        var applicableCookies = new List<string>();

        foreach (Cookie cookie in _cookies)
        {
            // Check if cookie is applicable for this URL
            if (IsCookieApplicable(cookie, uri))
            {
                applicableCookies.Add($"{cookie.Name}={cookie.Value}");
            }
        }

        return string.Join("; ", applicableCookies);
    }

    /// <summary>
    /// Clears all stored cookies
    /// </summary>
    public void ClearCookies()
    {
        logger.Information("Clearing all cookies. Previous count: {CookieCount}", _cookies.Count);
        _cookies.Clear();
    }

    /// <summary>
    /// Adds a cookie manually
    /// </summary>
    public void AddCookie(Cookie cookie)
    {
        UpdateOrAddCookie(cookie);
        logger.Information("Manually added cookie: {CookieName}={CookieValue}", cookie.Name, cookie.Value);
    }

    /// <summary>
    /// Removes a specific cookie by name and domain
    /// </summary>
    public bool RemoveCookie(string name, string domain = "", string path = "/")
    {
        Cookie? cookieToRemove = null;
        foreach (Cookie cookie in _cookies)
        {
            if (cookie.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                (string.IsNullOrEmpty(domain) || cookie.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase)) &&
                cookie.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
            {
                cookieToRemove = cookie;
                break;
            }
        }

        if (cookieToRemove != null)
        {
            _cookies.Remove(cookieToRemove);
            logger.Information("Removed cookie: {CookieName} from domain: {Domain}", name, cookieToRemove.Domain);
            return true;
        }

        return false;
    }

    private Cookie? ParseSetCookieHeader(string setCookieHeader, Uri requestUri)
    {
        try
        {
            // Split the Set-Cookie header to get the name=value pair and attributes
            var parts = setCookieHeader.Split(';');
            if (parts.Length == 0) return null;

            // Parse the name=value part
            var nameValuePart = parts[0].Trim();
            var equalIndex = nameValuePart.IndexOf('=');
            if (equalIndex == -1) return null;

            var name = nameValuePart.Substring(0, equalIndex).Trim();
            var value = nameValuePart.Substring(equalIndex + 1).Trim();

            var cookie = new Cookie(name, value);

            // Set default domain and path from request URI
            cookie.Domain = requestUri.Host;
            cookie.Path = "/";

            // Parse attributes
            for (int i = 1; i < parts.Length; i++)
            {
                var attribute = parts[i].Trim();
                var attrEqualIndex = attribute.IndexOf('=');

                if (attrEqualIndex != -1)
                {
                    var attrName = attribute.Substring(0, attrEqualIndex).Trim().ToLowerInvariant();
                    var attrValue = attribute.Substring(attrEqualIndex + 1).Trim();

                    switch (attrName)
                    {
                        case "domain":
                            cookie.Domain = attrValue;
                            break;
                        case "path":
                            cookie.Path = attrValue;
                            break;
                        case "expires":
                            if (DateTime.TryParse(attrValue, out var expires))
                            {
                                cookie.Expires = expires;
                            }
                            break;
                        case "max-age":
                            if (int.TryParse(attrValue, out var maxAge))
                            {
                                cookie.Expires = DateTime.UtcNow.AddSeconds(maxAge);
                            }
                            break;
                    }
                }
                else
                {
                    // Handle boolean attributes
                    var attrName = attribute.ToLowerInvariant();
                    switch (attrName)
                    {
                        case "secure":
                            cookie.Secure = true;
                            break;
                        case "httponly":
                            cookie.HttpOnly = true;
                            break;
                    }
                }
            }

            return cookie;
        }
        catch (Exception ex)
        {
            logger.Information("Failed to parse Set-Cookie header: {SetCookieHeader}. Error: {Error}", setCookieHeader, ex.Message);
            return null;
        }
    }

    private void UpdateOrAddCookie(Cookie newCookie)
    {
        // Remove existing cookie with same name, domain, and path
        Cookie? existingCookie = null;
        foreach (Cookie cookie in _cookies)
        {
            if (cookie.Name.Equals(newCookie.Name, StringComparison.OrdinalIgnoreCase) &&
                cookie.Domain.Equals(newCookie.Domain, StringComparison.OrdinalIgnoreCase) &&
                cookie.Path.Equals(newCookie.Path, StringComparison.OrdinalIgnoreCase))
            {
                existingCookie = cookie;
                break;
            }
        }

        if (existingCookie != null)
        {
            _cookies.Remove(existingCookie);
            logger.Information("Updated existing cookie: {CookieName}", newCookie.Name);
        }
        else
        {
            logger.Information("Added new cookie: {CookieName}", newCookie.Name);
        }

        _cookies.Add(newCookie);
    }

    private bool IsCookieApplicable(Cookie cookie, Uri uri)
    {
        // Check domain
        if (!uri.Host.EndsWith(cookie.Domain, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Check path
        if (!uri.AbsolutePath.StartsWith(cookie.Path, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Check if expired
        if (cookie.Expires != DateTime.MinValue && cookie.Expires < DateTime.UtcNow)
        {
            return false;
        }

        // Check secure flag
        if (cookie.Secure && uri.Scheme != "https")
        {
            return false;
        }

        return true;
    }
} 