namespace EClinicalWorksPoc;

public interface ICustomLogger
{
    void Information(string messageTemplate, bool logToConsole = true);
    void Information(string messageTemplate, object? arg, bool logToConsole = true);
    void Information(string messageTemplate, object? arg0, object? arg1, bool logToConsole = true);
    void Information(string messageTemplate, params object?[] args);
    void Information(string messageTemplate, bool logToConsole, params object?[] args);
    
    void Debug(string messageTemplate, bool logToConsole = true);
    void Debug(string messageTemplate, object? arg, bool logToConsole = true);
    void Debug(string messageTemplate, object? arg0, object? arg1, bool logToConsole = true);
    void Debug(string messageTemplate, params object?[] args);
    void Debug(string messageTemplate, bool logToConsole, params object?[] args);
    
    void Warning(string messageTemplate, bool logToConsole = true);
    void Warning(string messageTemplate, object? arg, bool logToConsole = true);
    void Warning(string messageTemplate, object? arg0, object? arg1, bool logToConsole = true);
    void Warning(string messageTemplate, params object?[] args);
    void Warning(string messageTemplate, bool logToConsole, params object?[] args);
    
    void Error(Exception? exception, string messageTemplate, bool logToConsole = true);
    void Error(Exception? exception, string messageTemplate, object? arg, bool logToConsole = true);
    void Error(Exception? exception, string messageTemplate, object? arg0, object? arg1, bool logToConsole = true);
    void Error(Exception? exception, string messageTemplate, params object?[] args);
    void Error(Exception? exception, string messageTemplate, bool logToConsole, params object?[] args);
    
    void Fatal(Exception? exception, string messageTemplate, bool logToConsole = true);
    void Fatal(Exception? exception, string messageTemplate, object? arg, bool logToConsole = true);
    void Fatal(Exception? exception, string messageTemplate, object? arg0, object? arg1, bool logToConsole = true);
    void Fatal(Exception? exception, string messageTemplate, params object?[] args);
    void Fatal(Exception? exception, string messageTemplate, bool logToConsole, params object?[] args);
} 