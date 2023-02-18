using Minecraft.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Packets.Receivers
{
    public sealed class DisconnectKickPacketReceiver : IPacketReceiver
    {
        public bool AllowOverride => true;

        public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
        {
            int packetLength = 3 + new MStream(rawPacket.Skip(1)).ReadString().Length;

            handler.Player.Connection.Close();

            return rawPacket.Skip(packetLength);
        }
    }
}
