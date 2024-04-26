using Minecraft.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Network.Packets.Receivers;

public sealed class ServerListPingPacketReceiver : IPacketReceiver
{
    public const int PacketLength = 2;

    public bool AllowOverride => true;

    public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
    {
        Player player = handler.Player;

        if (rawPacket.Count() >= PacketLength &&
            player.Connection is not null &&
            rawPacket.ElementAt(0) == (byte)PacketList.ServerListPing &&
            rawPacket.ElementAt(1) == 0x01)
        {
            player.Connection.SendPacket(new ServerListPingPacket('1', 51, "1.4.7", server.Config.Motd, server.Players.Count, server.Config.MaxPlayers));
        }

        player.Connection?.Close();

        return rawPacket.Skip(PacketLength);
    }
}
