using System.Text.Json.Nodes;
using System.Text.Json;
using System.IO;

using Minecraft.Tools;

namespace Minecraft.Configs
{
    public class ConfigManager
    {
        public string Motd { get; set; } = "A Minecraft Server";
        public ushort Port { get; private set; } = 25565;
        public string WorldName { get; private set; } = "world";
        public long WorldSeed { get; private set; } = 0;
        public bool OnlineMode { get; private set; } = false;
        public int MaxPlayers { get; private set; } = 20;
        private bool ConfigChanged;

        internal ConfigManager(string filename)
        {
            string text;
            if (!File.Exists(filename))
            {
                text = "{}";
                File.WriteAllText(filename, text);
            }
            else text = File.ReadAllText(filename);

            try
            {
                JsonObject config = JsonNode.Parse(text).AsObject();

                Motd = GetString(config, "motd", "A Minecraft Server");
                Port = (ushort)GetInteger(config, "port", 25565);
                OnlineMode = GetBoolean(config, "online-mode", false);
                MaxPlayers = GetInteger(config, "maxplayers", 20);
                WorldName = GetString(config, "worldname", "world");
                WorldSeed = GetLong(config, "worldseed", -1);

                if (ConfigChanged) File.WriteAllText(filename, config.ToJsonString(new JsonSerializerOptions() { WriteIndented = true }));
            }
            catch (JsonException e)
            {
                ConsoleWrapper.ConsoleWriter.WriteError(e);
            }
        }

        private string GetString(JsonObject node, string name, string defaultValue)
        {
            if (!node.TryGetPropertyValue(name, out JsonNode value) || value.GetValue<string>() is null)
            {
                node[name] = defaultValue;
                value = node[name];
                ConfigChanged = true;
            }

            return value.GetValue<string>();
        }

        private int GetInteger(JsonObject node, string name, int defaultValue)
        {
            if (!node.TryGetPropertyValue(name, out JsonNode value))
            {
                node[name] = defaultValue;
                value = node[name];
                ConfigChanged = true;
            }

            return value.GetValue<int>();
        }

        private bool GetBoolean(JsonObject node, string name, bool defaultValue)
        {
            if (!node.TryGetPropertyValue(name, out JsonNode value))
            {
                node[name] = defaultValue;
                value = node[name];
                ConfigChanged = true;
            }

            return value.GetValue<bool>();
        }

        private long GetLong(JsonObject node, string name, long defaultValue)
        {
            if (!node.TryGetPropertyValue(name, out JsonNode value))
            {
                node[name] = defaultValue;
                value = node[name];
                ConfigChanged = true;
            }

            return value.GetValue<long>();
        }
    }
}
