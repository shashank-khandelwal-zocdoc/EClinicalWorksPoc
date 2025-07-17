# EClinicalWorks POC

A proof-of-concept application for EClinicalWorks API integration with .NET 8.

## Features

- Configuration system with environment-specific settings
- Support for Development and Production environments
- EClinicalWorks cloud URL integration
- Secure credential management

## Configuration

The application uses appsettings.json files for configuration:

- `appsettings.json` - Default settings
- `appsettings.Development.json` - Development environment
- `appsettings.Production.json` - Production environment with ECW cloud URL

## Environment

Set `DOTNET_ENVIRONMENT` to control which configuration is loaded:
- Development (default)
- Production

## Usage

```bash
dotnet restore
dotnet run
```

## EClinicalWorks Integration

The application is configured to connect to:
- Production: https://txahjm10202023rgagjc7app.ecwcloud.com/
- Development: https://dev-api.eclinicalworks.com

Update the username and password in the respective appsettings files.