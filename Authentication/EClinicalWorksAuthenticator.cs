using EClinicalWorksPoc.Configuration;
using EClinicalWorksPoc.Logging;
using EClinicalWorksPoc.Parsing;
using EClinicalWorksPoc.Models;
using EClinicalWorksPoc.Http;
using System.Net;

namespace EClinicalWorksPoc.Authentication;

public class EClinicalWorksAuthenticator(EClinicalWorksSettings settings, ICustomLogger logger, HttpClient httpClient, IHtmlParser htmlParser, CookieManager cookieManager)
{
    // Expose the cookie manager for access to cookies
    public CookieManager CookieManager => cookieManager;

    // For backward compatibility, expose cookies directly
    public CookieCollection Cookies => cookieManager.Cookies;

    public async Task PerformAuthenticationAsync()
    {
        logger.Information("Starting EClinicalWorks authentication...");

        try
        {
            // Log authentication attempt (without password for security)
            logger.Information("Authenticating with EClinicalWorks at {BaseUrl} for user {Username}", 
                settings.BaseUrl, settings.Username);

            // Make HTTP GET request to login page and parse the response
            await GetLoginPageAsync();

            logger.Information("EClinicalWorks authentication completed successfully");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "EClinicalWorks authentication failed");
            throw;
        }
    }

    private async Task GetLoginPageAsync()
    {
        var loginUrl = $"{settings.BaseUrl}{EClinicalWorksConstants.LoginPageUrl}";

        logger.Information("Making GET request to login page: {LoginUrl}", loginUrl);

        var response = await httpClient.GetAsync(loginUrl);

        logger.Information("Received response from login page - Status: {StatusCode}", response.StatusCode);

        // Extract and persist cookies from the response using CookieManager
        cookieManager.ExtractAndPersistCookies(response, loginUrl);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            logger.Information("Login page response received successfully. Content length: {ContentLength} characters",
                content.Length);
            logger.Information("Login page content: {Content}", content, false);

            // Parse the HTML content using the HTML parser
            var loginView = htmlParser.Parse<NewLoginView>(content);

            logger.Information(
                "HTML parsing completed. Found CSRF Token: {HasCsrfToken}, RSA Key Script: {HasRsaKey}, Version: {HasVersion}",
                loginView.CsrfToken, loginView.RawTextWithRsaPubKey, loginView.Version);

            return;
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        logger.Warning("Login page request failed with status {StatusCode}. Error content: {ErrorContent}",
            response.StatusCode, errorContent);
        response.EnsureSuccessStatusCode();
    }
} 