# EClinicalWorks POC

A proof-of-concept application for EClinicalWorks API integration with .NET 8.

## Features

- Simple configuration system with a single settings file
- EClinicalWorks cloud URL integration
- Secure credential management (credentials not committed to git)

## Setup

1. Copy the example configuration file:
   ```bash
   cp appsettings.example.json appsettings.json
   ```

2. Edit `appsettings.json` and update the credentials:
   ```json
   {
     "EClinicalWorks": {
       "BaseUrl": "client-base-url",
       "Username": "client-actual-username",
       "Password": "client-actual-password"
     }
   }
   ```

## Configuration

The application uses a single `appsettings.json` file for configuration. This file contains sensitive credentials and is excluded from git commits for security.

## Usage

```bash
dotnet restore
dotnet run
```

## EClinicalWorks Integration

The application is configured to connect to: https://txahjm10202023rgagjc7app.ecwcloud.com/

**Important**: Never commit your actual `appsettings.json` file with real credentials. Use `appsettings.example.json` as a template.