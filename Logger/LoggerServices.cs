using System;
using System.IO;

namespace Logger;

public class LoggerService : ILoggerService
{
    private readonly string path = "Logs/app.log";


    public void LogInformation(string message)
    {
        WriteLog("INFO", message);
    }


    public void LogWarning(string message)
    {
        WriteLog("WARNING", message);
    }


    public void LogError(string message)
    {
        WriteLog("ERROR", message);
    }


    private void WriteLog(string type, string message)
    {
        Directory.CreateDirectory("Logs");

        string text =
            $"{DateTime.Now} [{type}] {message}";

        File.AppendAllText(path, text + Environment.NewLine);
    }
}