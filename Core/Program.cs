using Microsoft.Extensions.Configuration;
using EClinicalWorksPoc.Core;
using EClinicalWorksPoc.Authentication;
using EClinicalWorksPoc.Configuration;
using EClinicalWorksPoc.Logging;
using EClinicalWorksPoc.Parsing;
using EClinicalWorksPoc.Http;
using Serilog;
using System.Diagnostics.CodeAnalysis;

// Configure Serilog logger
var executablePath = AppContext.BaseDirectory;
var logsPath = Path.Combine(executablePath, "logs");

// Ensure logs directory exists
Directory.CreateDirectory(logsPath);

// Create a unique log file name for each run
var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
var logFileName = $"EClinicalWorksPoc_{timestamp}.log";
var logFilePath = Path.Combine(logsPath, logFileName);

// Configure separate file and console loggers
var fileLogger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(logFilePath, 
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Infinite) // Don't roll the file since we create a new one each run
    .CreateLogger();

var consoleLogger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

// Set the global logger to file logger for compatibility
Log.Logger = fileLogger;

// Create custom logger that combines both
var customLogger = new CustomLogger(fileLogger, consoleLogger);

try
{
    customLogger.Information("Application starting up...");
    customLogger.Information("Log file created at: {LogFilePath}", logFilePath);

    // Build configuration from appsettings.json
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    customLogger.Information("Configuration loaded from appsettings.json");

    // Bind the EClinicalWorks section to our settings class
    var eclinicalWorksSettings = new EClinicalWorksSettings();
    #pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access
    configuration.GetSection(EClinicalWorksSettings.SectionName).Bind(eclinicalWorksSettings);
    #pragma warning restore IL2026

    customLogger.Information("EClinicalWorks settings bound successfully");

    // Create HttpClient for making HTTP requests
    using var httpClient = new HttpClient();

    // Create HTML parser for deserializing HTML content
    var htmlParser = new HtmlParser();

    // Create cookie manager for handling HTTP cookies
    var cookieManager = new CookieManager(customLogger);

    // Create the authenticator
    var authenticator = new EClinicalWorksAuthenticator(eclinicalWorksSettings, customLogger, httpClient, htmlParser, cookieManager);
    
    // Create and run the application
    var application = new EClinicalWorksApplication(eclinicalWorksSettings, customLogger, authenticator);
    await application.RunAsync();

    customLogger.Information("Application completed successfully");
}
catch (Exception ex)
{
    customLogger.Fatal(ex, "Application terminated unexpectedly");
    Environment.ExitCode = 1;
}
finally
{
    Log.CloseAndFlush();
} 