using System.Collections.Generic;

namespace Minecraft.World
{
    public class DataPackList
    {
        public readonly HashSet<string> Disabled;
        public readonly HashSet<string> Enabled;

        internal DataPackList()
        {
            Disabled = new HashSet<string>();
            Enabled = new HashSet<string>();
            Enabled.Add("vanilla");
        }
    }
}
