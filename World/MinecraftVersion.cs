namespace Minecraft.World
{
    public class MinecraftVersion
    {
        public readonly int Id;
        public readonly string Name;
        public readonly byte Snapshot;

        internal MinecraftVersion(int id, string name, byte snapshot)
        {
            Id = id;
            Name = name;
            Snapshot = snapshot;
        }
    }
}
