using System;
using System.IO;
using System.Text;

namespace Minecraft.Tools
{
    public class FilesHandler
    {
        private readonly string Base;

        private readonly string Logs;
        private readonly string Plugins;
        private readonly string Configs;
        private readonly string LatestLog;

        private const string LogsDirectory = "logs";
        private const string PluginsDirectory = "plugins";
        private const string ConfigsDirectory = "configs";
        private const string LatestLogFile = "latest.log";

        private FileStream? LatestLogStream;
        private bool IsInitialized = false;

        internal FilesHandler()
        {
            Base = Directory.GetCurrentDirectory();
            Logs = Path.Combine(GetBase(), LogsDirectory);
            Plugins = Path.Combine(GetBase(), PluginsDirectory);
            Configs = Path.Combine(GetBase(), ConfigsDirectory);
            LatestLog = Path.Combine(GetLogs(), LatestLogFile);
        }

        internal void Initialize()
        {
            if (!Directory.Exists(GetLogs())) Directory.CreateDirectory(GetLogs());
            if (File.Exists(GetLatestLog()))
            {
                DateTime date = File.GetLastWriteTime(GetLatestLog());
                for (int i = 1; ; i++)
                {
                    string path = Path.Combine(GetLogs(), EffectiveTools.GetShortTimeStamp(date) + "-" + i + ".log");
                    if (File.Exists(path)) continue;
                    File.Move(GetLatestLog(), path, true);
                    break;
                }
            }
            LatestLogStream = File.Open(GetLatestLog(), FileMode.Create, FileAccess.Write, FileShare.Read);
            IsInitialized = true;
        }

        internal void Stop()
        {
            if (IsInitialized && LatestLogStream is not null)
            {
                IsInitialized = false;
                LatestLogStream.Close();
                LatestLogStream = null;
                File.SetLastWriteTime(GetLatestLog(), DateTime.Now);
            }
        }

        public void WriteToLog(byte[] buffer, int offset, int count)
        {
            if (IsInitialized && LatestLogStream is not null)
            {
                LatestLogStream.Write(buffer, offset, count);
                LatestLogStream.WriteByte(0x0A);
                LatestLogStream.Flush();
            }
        }

        public void WriteToLog(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            WriteToLog(buffer, 0, buffer.Length);
        }

        public string GetBase()
        {
            return Base;
        }

        public string GetLogs()
        {
            return Logs;
        }

        public string GetPlugins()
        {
            return Plugins;
        }

        public string GetConfigs()
        {
            return Configs;
        }

        public string GetLatestLog()
        {
            return LatestLog;
        }
    }
}
