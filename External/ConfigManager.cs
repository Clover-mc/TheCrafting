using System;
using System.IO;

namespace Minecraft.External
{
    internal class ConfigManager
    {
        public string MOTD { get; private set; } = "[!WARNING!] Config File Not Found!";
        public int PORT { get; private set; } = 25565;
        public string WorldName { get; private set; } = "world";

        internal ConfigManager(string filename)
        {
            if (!File.Exists(filename))
            {
                Tools.ConsoleOutputWrapper.Write(ConsoleColor.Red, MOTD, Tools.ConsoleOutputLevel.LogLevel.ERROR);
                return;
            }
            string[] values = File.ReadAllLines(filename);
            MOTD = values[0];
            PORT = int.Parse(values[1]);
            WorldName = values[2];
        }
    }
}
