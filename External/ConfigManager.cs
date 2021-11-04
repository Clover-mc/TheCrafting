namespace Minecraft.External
{
    internal class ConfigManager
    {
        public string MOTD { get; private set; }
        public int PORT { get; private set; }

        internal ConfigManager(string filename)
        {
            string[] values = File.ReadAllLines(filename);
            MOTD = values[0];
            PORT = int.Parse(values[1]);
        }
    }
}
