using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Packets
{
    public class LoginRequestPacket : IPacket
    {
        private MStream Stream;

        public PacketList Id => PacketList.LOGIN_REQUEST;
        public int PacketLength { get; private set; }
        public IEnumerable<byte> Raw => Stream.Array;

        public LoginRequestPacket(int entityID, LevelType level_type, Gamemode gamemode, Dimension dimension, Difficulty difficulty, byte max_players)
        {
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(entityID);
            Stream.Write(level_type.Name);
            Stream.Write((byte)gamemode);
            Stream.Write((byte)dimension);
            Stream.Write((byte)difficulty);
            Stream.WriteByte(0);
            Stream.Write(max_players);

            PacketLength = 12 + level_type.Name.Length * 2; // Every string is double-sized
        }
    }
}
