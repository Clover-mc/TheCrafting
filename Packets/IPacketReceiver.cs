using System.Collections.Generic;

namespace Minecraft.Packets
{
    public interface IPacketReceiver
    {
        public bool AllowOverride { get; }
        public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket);
    }
}
