using Microsoft.Extensions.Configuration;

namespace EClinicalWorksPoc;

public class EClinicalWorksApplication(EClinicalWorksSettings settings, ICustomLogger logger, EClinicalWorksAuthenticator authenticator)
{
    public async Task RunAsync()
    {
        logger.Information("EClinicalWorks application starting...");

        try
        {
            LogConfiguration();
            await ProcessApplicationLogicAsync();
            logger.Information("EClinicalWorks application completed successfully");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred during application execution");
            throw;
        }
    }

    private void LogConfiguration()
    {
        logger.Information("EClinicalWorks configuration");
        logger.Information("Base URL: {BaseUrl}", settings.BaseUrl);
        logger.Information("Username: {Username}", settings.Username);
        logger.Information("Password: {Password}", settings.Password);
    }

    private async Task ProcessApplicationLogicAsync()
    {
        logger.Information("Starting process application logic...");
        
        // Perform authentication using the injected authenticator
        await authenticator.PerformAuthenticationAsync();
        
        logger.Information("Process application logic completed");
    }
} 