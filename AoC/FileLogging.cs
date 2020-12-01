using System;
using System.IO;
using Serilog;

namespace AoC
{
    /// <summary>
    /// Use for file based logging, for long running tasks.
    /// </summary>
    public static class FileLogging
    {
        private static ILogger? _logger;

        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    Initialize();
                }

                return _logger!;
            }
        }

        public static void Initialize(bool includeConsole = false, Action<LoggerConfiguration>? customise = null)
        {
            var logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".logs", "aoc2020");
            Directory.CreateDirectory(logFolderPath);

            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logFolderPath, "aoc.txt"), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);

            if (includeConsole)
            {
                loggerConfiguration.WriteTo.Console();
            }

            customise?.Invoke(loggerConfiguration);

            _logger = loggerConfiguration.CreateLogger();
        }
    }
}
