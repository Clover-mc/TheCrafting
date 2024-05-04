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
        var count = rawPacket.Count();

        if (count >= 1 &&
            player.Connection is not null &&
            rawPacket.ElementAt(0) == (byte)PacketList.ServerListPing)
        {
            if (count >= 2 && rawPacket.ElementAt(1) == 0x01)
            {
                player.Connection.SendPackets(ServerListPingPacket.GetResponse((byte)'1', 51, "1.4.7", server.Config.Motd, server.Players.Count(), server.Config.MaxPlayers));
            }
            else if (count == 1)
            {
                player.Connection.SendPackets(ServerListPingPacket.GetLegacyResponse("Minecraft 1.4 required",
                    server.Players.Count(), server.Config.MaxPlayers));
            }
        }

        player.Connection?.Close();

        return rawPacket.Skip(PacketLength);
    }
}
