using Microsoft.Extensions.Logging;
using Serilog;

namespace eCommerce.Shared.Library.Logs;

public static class LogException
{
    public static void LogExceptions(Exception exception)
    {
        LogToFile(exception.Message);
        LogToConsole(exception.Message);
        LogToDebugger(exception.Message);
    }
    
    //LogToFile()
    public static void LogToFile(string message) => Log.Information(message);
    //LogToConsole
    public static void LogToConsole(string message) => Log.Information(message);
    //LogToDebugger
    public static void LogToDebugger(string message) => Log.Information(message);
}