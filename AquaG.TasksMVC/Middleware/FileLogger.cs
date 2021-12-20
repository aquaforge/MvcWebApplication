using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AquaG.TasksMVC.Middleware
{
    public class FileLogger : ILogger
    {
        private readonly string fileDir;
        private static readonly object _lock = new object();

        public FileLogger(string startDir = "")
        {
            if (startDir == "" || !Directory.Exists(startDir))
                startDir = Directory.GetCurrentDirectory();

            startDir = Path.Combine(startDir, "AppDailyLogs");
            if (!Directory.Exists(startDir))
                Directory.CreateDirectory(startDir);

            fileDir = startDir;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                try
                {
                    lock (_lock)
                    {
                        string text = $"<br>{string.Format("{0:HH:mm:ss}", DateTime.Now)} {formatter(state, exception)}{Environment.NewLine}";
                        string filePath = Path.Combine(fileDir, string.Format(@"{0:yyyy-MM-dd}", DateTime.Now) + ".log");
                        File.AppendAllText(filePath, text);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"GeneralFileLogger.Log: {ex.Message}");
                }
            }
        }
    }
}