using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AquaG.TasksMVC.Middleware
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath = "")
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}