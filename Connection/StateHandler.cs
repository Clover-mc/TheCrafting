using Minecraft.Entities;

namespace Minecraft.Connection
{
    internal abstract class StateHandler
    {
        internal abstract void Handle(Player player, byte[] packet);
    }
}
