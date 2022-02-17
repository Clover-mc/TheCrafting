using System.Collections.Generic;

namespace Minecraft
{
    public interface IServer
    {
        public void Start(string[] args);
        public void Stop();
        public bool GetLogIncoming();
        public bool GetLogOutgoing();
        public External.ConfigManager GetConfigManager();
        public bool IsLaunched();
        public List<World.World> GetWorlds();
    }
}
