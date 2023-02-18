using Minecraft.Tools;

namespace Minecraft
{
    public class LevelType : Enumeration
    {
        public static readonly LevelType DEFAULT = new LevelType(0, "default");
        public static readonly LevelType FLAT = new LevelType(1, "flat");
        public static readonly LevelType LARGE_BIOMES = new LevelType(2, "largeBiomes");

        public LevelType(int id, string type) : base(id, type) { }
    }
}
