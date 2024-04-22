using System;
using System.IO;
using System.Text;

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

    FileStream? _latestLogStream;

    internal FilesHandler()
    {
        Base = Directory.GetCurrentDirectory();

        Logs = Path.Combine(Base, LogsDirectory);
        Plugins = Path.Combine(Base, PluginsDirectory);
        Configs = Path.Combine(Base, ConfigsDirectory);
        LatestLog = Path.Combine(Logs, LatestLogFile);
    }

    internal void Initialize()
    {
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

        _latestLogStream = File.Open(LatestLog, FileMode.Create, FileAccess.Write, FileShare.Read);
    }

    internal void Stop()
    {
        if (_latestLogStream is not null)
        {
            _latestLogStream.Flush();
            _latestLogStream.Close();
            _latestLogStream = null;
        }
    }

    public void WriteToLog(byte[] buffer, int offset, int count)
    {
        _latestLogStream?.Write(buffer, offset, count);
    }

    public void WriteToLog(string str)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(str + Environment.NewLine);
        WriteToLog(buffer, 0, buffer.Length);
    }

    public void WriteToLogRaw(string str)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(str);
        WriteToLog(buffer, 0, buffer.Length);
    }
}
