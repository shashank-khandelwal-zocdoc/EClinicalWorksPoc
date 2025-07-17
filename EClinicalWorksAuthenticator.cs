namespace EClinicalWorksPoc;

public class EClinicalWorksAuthenticator(EClinicalWorksSettings settings, ICustomLogger logger, HttpClient httpClient)
{
    public async Task PerformAuthenticationAsync()
    {
        logger.Information("Starting EClinicalWorks authentication...");

        try
        {
            // Log authentication attempt (without password for security)
            logger.Information("Authenticating with EClinicalWorks at {BaseUrl} for user {Username}", 
                settings.BaseUrl, settings.Username);

            // Make HTTP GET request to login page
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
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            logger.Information("Login page response received successfully. Content length: {ContentLength} characters", 
                content.Length);
            logger.Information("Login page content: {Content}", content, false);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            logger.Warning("Login page request failed with status {StatusCode}. Error content: {ErrorContent}", 
                response.StatusCode, errorContent);
            response.EnsureSuccessStatusCode(); // This will throw if not successful
        }
    }
} 