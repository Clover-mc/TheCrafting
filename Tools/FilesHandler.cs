using System;
using System.IO;

namespace Minecraft.Tools;

public class FilesHandler
{
    public string Base { get; }

    public string Logs { get; }

    public string Plugins { get; }
    
    public string Configs { get; }
    
    public string LatestLog { get; }

    public const string LogsDirectory = "logs";
    public const string PluginsDirectory = "plugins";
    public const string ConfigsDirectory = "configs";
    public const string LatestLogFile = "latest.log";

    bool _initialized;

    internal FilesHandler()
    {
        Base = Directory.GetCurrentDirectory();

        Logs = Path.Combine(Base, LogsDirectory);
        Plugins = Path.Combine(Base, PluginsDirectory);
        Configs = Path.Combine(Base, ConfigsDirectory);
        LatestLog = Path.Combine(Logs, LatestLogFile);
    }

    public void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        if (!Directory.Exists(Logs))
        {
            Directory.CreateDirectory(Logs);
        }

        if (File.Exists(LatestLog))
        {
            DateTime date = File.GetLastWriteTime(LatestLog);
            for (int i = 1; ; i++)
            {
                string path = Path.Combine(Logs, $"log-{date:MM-dd-yyyy}-{i}.log");
                if (File.Exists(path))
                {
                    continue;
                }

                File.Move(LatestLog, path, true);
                break;
            }
        }

        _initialized = true;
    }
}
