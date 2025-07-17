using Microsoft.Extensions.Configuration;
using EClinicalWorksPoc;

// Build configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Bind the EClinicalWorks section to our settings class
var eclinicalWorksSettings = new EClinicalWorksSettings();
configuration.GetSection(EClinicalWorksSettings.SectionName).Bind(eclinicalWorksSettings);

// Display the loaded settings
Console.WriteLine("EClinicalWorks Configuration:");
Console.WriteLine($"Base URL: {eclinicalWorksSettings.BaseUrl}");
Console.WriteLine($"Username: {eclinicalWorksSettings.Username}");
Console.WriteLine($"Password: {new string('*', eclinicalWorksSettings.Password.Length)}"); // Mask password for security

// Example of how to use the settings in your application
Console.WriteLine("\nConfiguration loaded successfully!");
Console.WriteLine("You can now use these settings to configure your EClinicalWorks API client.");