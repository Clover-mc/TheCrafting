using System;
using System.IO;

namespace Minecraft.External
{
    public class ConfigManager
    {
        private string MOTD = "A Minecraft Server";
        private int PORT = 25565;
        private string WorldName = "world";

        internal ConfigManager(string filename)
        {
            if (!File.Exists(filename))
            {
                StreamWriter file = File.CreateText(filename);
                file.Write(GetMOTD() + '\n' + GetPort() + '\n' + GetWorldName());
                file.Flush();
                file.Close();
            }
            string[] values = File.ReadAllLines(filename);
            MOTD = values[0];
            PORT = int.Parse(values[1]);
            WorldName = values[2];
        }

        public string GetMOTD()
        {
            return MOTD;
        }
        public int GetPort()
        {
            return PORT;
        }
        public string GetWorldName()
        {
            return WorldName;
        }

        public void SetMOTD(string motd)
        {
            MOTD = motd;
        }
        public void SetPort(int port)
        {
            PORT = port;
        }
        public void SetWorldName(string worldname)
        {
            WorldName = worldname;
        }
    }
}
