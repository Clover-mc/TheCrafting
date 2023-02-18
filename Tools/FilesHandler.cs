using System;
using System.IO;
using System.Text;

namespace Minecraft.Tools
{
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

        private FileStream? LatestLogStream;
        private bool IsInitialized;

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
            if (!Directory.Exists(Logs)) Directory.CreateDirectory(Logs);
            if (File.Exists(LatestLog))
            {
                DateTime date = File.GetLastWriteTime(LatestLog);
                for (int i = 1; ; i++)
                {
                    string path = Path.Combine(Logs, EffectiveTools.GetDateStamp(date) + "-" + i + ".log");
                    if (File.Exists(path)) continue;
                    File.Move(LatestLog, path, true);
                    break;
                }
            }
            LatestLogStream = File.Open(LatestLog, FileMode.Create, FileAccess.Write, FileShare.Read);
            IsInitialized = true;
        }

        internal void Stop()
        {
            if (IsInitialized && LatestLogStream is not null)
            {
                IsInitialized = false;
                LatestLogStream.Close();
                LatestLogStream = null;
                File.SetLastWriteTime(LatestLog, DateTime.Now);
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

        public void WriteToLogRaw(byte[] buffer, int offset, int count)
        {
            if (IsInitialized && LatestLogStream is not null)
            {
                LatestLogStream.Write(buffer, offset, count);
                LatestLogStream.Flush();
            }
        }

        public void WriteToLog(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            WriteToLog(buffer, 0, buffer.Length);
        }

        public void WriteToLogRaw(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            WriteToLogRaw(buffer, 0, buffer.Length);
        }
    }
}
