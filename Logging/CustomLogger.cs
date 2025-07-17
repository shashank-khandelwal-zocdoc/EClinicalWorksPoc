using Serilog;
using ILogger = Serilog.ILogger;

namespace EClinicalWorksPoc.Logging;

public class CustomLogger(ILogger fileLogger, ILogger consoleLogger) : ICustomLogger
{
    private readonly ILogger _fileLogger = fileLogger;
    private readonly ILogger _consoleLogger = consoleLogger;

    // Information methods
    public void Information(string messageTemplate, bool logToConsole = true)
    {
        _fileLogger.Information(messageTemplate);
        if (logToConsole)
            _consoleLogger.Information(messageTemplate);
    }

    public void Information(string messageTemplate, object? arg, bool logToConsole = true)
    {
        _fileLogger.Information(messageTemplate, arg);
        if (logToConsole)
            _consoleLogger.Information(messageTemplate, arg);
    }

    public void Information(string messageTemplate, object? arg0, object? arg1, bool logToConsole = true)
    {
        _fileLogger.Information(messageTemplate, arg0, arg1);
        if (logToConsole)
            _consoleLogger.Information(messageTemplate, arg0, arg1);
    }

    public void Information(string messageTemplate, params object?[] args)
    {
        _fileLogger.Information(messageTemplate, args);
        _consoleLogger.Information(messageTemplate, args);
    }

    public void Information(string messageTemplate, bool logToConsole, params object?[] args)
    {
        _fileLogger.Information(messageTemplate, args);
        if (logToConsole)
            _consoleLogger.Information(messageTemplate, args);
    }

    // Debug methods
    public void Debug(string messageTemplate, bool logToConsole = true)
    {
        _fileLogger.Debug(messageTemplate);
        if (logToConsole)
            _consoleLogger.Debug(messageTemplate);
    }

    public void Debug(string messageTemplate, object? arg, bool logToConsole = true)
    {
        _fileLogger.Debug(messageTemplate, arg);
        if (logToConsole)
            _consoleLogger.Debug(messageTemplate, arg);
    }

    public void Debug(string messageTemplate, object? arg0, object? arg1, bool logToConsole = true)
    {
        _fileLogger.Debug(messageTemplate, arg0, arg1);
        if (logToConsole)
            _consoleLogger.Debug(messageTemplate, arg0, arg1);
    }

    public void Debug(string messageTemplate, params object?[] args)
    {
        _fileLogger.Debug(messageTemplate, args);
        _consoleLogger.Debug(messageTemplate, args);
    }

    public void Debug(string messageTemplate, bool logToConsole, params object?[] args)
    {
        _fileLogger.Debug(messageTemplate, args);
        if (logToConsole)
            _consoleLogger.Debug(messageTemplate, args);
    }

    // Warning methods
    public void Warning(string messageTemplate, bool logToConsole = true)
    {
        _fileLogger.Warning(messageTemplate);
        if (logToConsole)
            _consoleLogger.Warning(messageTemplate);
    }

    public void Warning(string messageTemplate, object? arg, bool logToConsole = true)
    {
        _fileLogger.Warning(messageTemplate, arg);
        if (logToConsole)
            _consoleLogger.Warning(messageTemplate, arg);
    }

    public void Warning(string messageTemplate, object? arg0, object? arg1, bool logToConsole = true)
    {
        _fileLogger.Warning(messageTemplate, arg0, arg1);
        if (logToConsole)
            _consoleLogger.Warning(messageTemplate, arg0, arg1);
    }

    public void Warning(string messageTemplate, params object?[] args)
    {
        _fileLogger.Warning(messageTemplate, args);
        _consoleLogger.Warning(messageTemplate, args);
    }

    public void Warning(string messageTemplate, bool logToConsole, params object?[] args)
    {
        _fileLogger.Warning(messageTemplate, args);
        if (logToConsole)
            _consoleLogger.Warning(messageTemplate, args);
    }

    // Error methods
    public void Error(Exception? exception, string messageTemplate, bool logToConsole = true)
    {
        _fileLogger.Error(exception, messageTemplate);
        if (logToConsole)
            _consoleLogger.Error(exception, messageTemplate);
    }

    public void Error(Exception? exception, string messageTemplate, object? arg, bool logToConsole = true)
    {
        _fileLogger.Error(exception, messageTemplate, arg);
        if (logToConsole)
            _consoleLogger.Error(exception, messageTemplate, arg);
    }

    public void Error(Exception? exception, string messageTemplate, object? arg0, object? arg1, bool logToConsole = true)
    {
        _fileLogger.Error(exception, messageTemplate, arg0, arg1);
        if (logToConsole)
            _consoleLogger.Error(exception, messageTemplate, arg0, arg1);
    }

    public void Error(Exception? exception, string messageTemplate, params object?[] args)
    {
        _fileLogger.Error(exception, messageTemplate, args);
        _consoleLogger.Error(exception, messageTemplate, args);
    }

    public void Error(Exception? exception, string messageTemplate, bool logToConsole, params object?[] args)
    {
        _fileLogger.Error(exception, messageTemplate, args);
        if (logToConsole)
            _consoleLogger.Error(exception, messageTemplate, args);
    }

    // Fatal methods
    public void Fatal(Exception? exception, string messageTemplate, bool logToConsole = true)
    {
        _fileLogger.Fatal(exception, messageTemplate);
        if (logToConsole)
            _consoleLogger.Fatal(exception, messageTemplate);
    }

    public void Fatal(Exception? exception, string messageTemplate, object? arg, bool logToConsole = true)
    {
        _fileLogger.Fatal(exception, messageTemplate, arg);
        if (logToConsole)
            _consoleLogger.Fatal(exception, messageTemplate, arg);
    }

    public void Fatal(Exception? exception, string messageTemplate, object? arg0, object? arg1, bool logToConsole = true)
    {
        _fileLogger.Fatal(exception, messageTemplate, arg0, arg1);
        if (logToConsole)
            _consoleLogger.Fatal(exception, messageTemplate, arg0, arg1);
    }

    public void Fatal(Exception? exception, string messageTemplate, params object?[] args)
    {
        _fileLogger.Fatal(exception, messageTemplate, args);
        _consoleLogger.Fatal(exception, messageTemplate, args);
    }

    public void Fatal(Exception? exception, string messageTemplate, bool logToConsole, params object?[] args)
    {
        _fileLogger.Fatal(exception, messageTemplate, args);
        if (logToConsole)
            _consoleLogger.Fatal(exception, messageTemplate, args);
    }
} 