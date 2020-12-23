using System;
using System.IO;
using Crayon;
using Serilog;

namespace AoC
{
    /// <summary>
    /// Use for file based logging, for long running tasks.
    /// </summary>
    public static class FileLogging
    {
        private static readonly Lazy<ILogger> LazyLogger = new(() => CreateLogger());

        public static ILogger Logger { get; } = LazyLogger.Value;

        public static ILogger CreateLogger(
            string logFileName = "aoc",
            bool includeConsole = false,
            RollingInterval rollingInterval = RollingInterval.Infinite,
            Action<LoggerConfiguration>? customise = null)
        {
            var logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aoc", "logs");
            Directory.CreateDirectory(logFolderPath);
            var logFilePath = Path.ChangeExtension(Path.Combine(logFolderPath, logFileName), ".txt");

            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.File(logFilePath, rollingInterval: rollingInterval, rollOnFileSizeLimit: true);

            if (includeConsole)
            {
                loggerConfiguration.WriteTo.Console();
            }

            customise?.Invoke(loggerConfiguration);

            var logger = loggerConfiguration.CreateLogger();
            Console.WriteLine($"(Setup logging to file {logFilePath})".BrightBlack());
            return logger;
        }
    }
}
