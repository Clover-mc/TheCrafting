using Minecraft.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Packets.Receivers
{
    public sealed class ServerListPingPacketReceiver : IPacketReceiver
    {
        public const int PACKET_LENGTH = 2;

        public bool AllowOverride => true;

        public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
        {
            Player player = handler.Player;

            if (rawPacket.ElementAt(1) == 1)
                player.Connection.SendPacket(new ServerListPingPacket('1', 51, "1.4.7", server.Config.Motd, server.Players.Count, server.Config.MaxPlayers));

            player.Connection.Close();
            return rawPacket.Skip(PACKET_LENGTH);
        }
    }
}
