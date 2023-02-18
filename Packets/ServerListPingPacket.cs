using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Packets
{
    public class ServerListPingPacket : IPacket
    {
        private MStream Stream;

        public char PingVersion { get; private set; }
        public int ProtocolVersion { get; private set; }
        public string VersionName { get; private set; }
        public string Description { get; private set; }
        public int OnlinePlayers { get; private set; }
        public int MaxPlayers { get; private set; }

        public PacketList Id => PacketList.SERVER_LIST_PING;
        public int PacketLength => -1;
        public IEnumerable<byte> Raw => Stream.Array;

        public ServerListPingPacket(char   ping_version, int protocol_version, string version_name,
                                    string description,  int online_players,   int    max_players)
        {
            PingVersion = ping_version;
            ProtocolVersion = protocol_version;
            VersionName = version_name;
            Description = description;
            OnlinePlayers = online_players;
            MaxPlayers = max_players;

            Stream = new MStream();
            Stream.WriteByte((byte)PacketList.DISCONNECT_KICK);
            Stream.Write(
                '\xA7' + PingVersion.ToString() +
                '\0' + ProtocolVersion +
                '\0' + VersionName +
                '\0' + Description +
                '\0' + OnlinePlayers +
                '\0' + MaxPlayers
            );
        }
    }
}
