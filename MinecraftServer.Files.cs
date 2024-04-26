using System;
using System.IO;
using System.Linq;

namespace Minecraft;

public partial class MinecraftServer
{
    public sealed class FilePaths
    {
        public string Base { get; } = Directory.GetCurrentDirectory();

        public string Logs { get; }

        public string Plugins { get; }

        public string Configs { get; }

        public string LatestLog { get; }

        public const string LogsDirectory = "logs";
        public const string PluginsDirectory = "plugins";
        public const string ConfigsDirectory = "configs";
        public const string LatestLogFile = "latest.log";

        internal FilePaths()
        {
            Logs = Path.Combine(Base, LogsDirectory);
            Plugins = Path.Combine(Base, PluginsDirectory);
            Configs = Path.Combine(Base, ConfigsDirectory);
            LatestLog = Path.Combine(Logs, LatestLogFile);
        }

        public bool ArchiveLatestLog()
        {
            if (!Directory.Exists(Logs))
            {
                Directory.CreateDirectory(Logs);
                return true;
            }

            if (!File.Exists(LatestLog))
            {
                return true;
            }

            DateTime date = File.GetLastWriteTime(LatestLog);
            string[] filesWithSameDateAsLatest = Directory.GetFiles(LogsDirectory, $"log-{date:MM-dd-yyyy}-*.log");
            for (int i = 1; ; i++)
            {
                string path = Path.Combine(Logs, $"log-{date:MM-dd-yyyy}-{i}.log");
                if (filesWithSameDateAsLatest.Contains(path))
                {
                    continue;
                }

                try
                {
                    File.Move(LatestLog, path, true);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
    }
}
