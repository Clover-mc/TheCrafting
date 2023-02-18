using Minecraft.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Packets.Receivers
{
    public class PlayerPacketReceiver : IPacketReceiver
    {
        public bool AllowOverride => true;

        public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
        {
            Player player = handler.Player;
            PlayerPacket packet = new PlayerPacket(rawPacket);

            player.IsOnGround = packet.IsOnGround;

            return rawPacket.Skip(packet.PacketLength);
        }
    }
}
