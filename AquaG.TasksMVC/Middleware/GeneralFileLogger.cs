using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AquaG.TasksMVC.Middleware
{
    public static class GeneralFileLogger
    {
        private static readonly object _lock = new object();

        public static void Log(string logText)
        {
            try
            {
                lock (_lock)
                {
                    string text = $"<br>{string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now)} {logText}{Environment.NewLine}";
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "_error_.log");
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