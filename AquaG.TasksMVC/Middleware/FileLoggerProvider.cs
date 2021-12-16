using Microsoft.Extensions.Logging;

namespace AquaG.TasksMVC.Middleware
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string path;

        public FileLoggerProvider(string _path)
        {
            path = _path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose() { }
    }
}