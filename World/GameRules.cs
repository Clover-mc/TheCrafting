using System.Collections.Generic;
using System.Linq;

namespace Minecraft.World
{

    public class GameRules
    {
        private readonly HashSet<GameRule> Gamerules;

        internal GameRules()
        {
            Gamerules = new HashSet<GameRule>();
        }

        public GameRule? Get(string name)
        {
            return Gamerules.FirstOrDefault(g => g.GetName() == name);
        }

        public void Set(string name, string value)
        {
            //
        }
    }
}
