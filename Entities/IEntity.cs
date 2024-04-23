namespace Minecraft.Entities
{
    public interface IEntity
    {
        public uint EntityId { get; set; }
        public Location Location { get; set; }

        //public static T Create<T>(MinecraftServer server, Location location) where T : Entity
        //{
            
        //}
    }
}
