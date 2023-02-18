using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Packets
{
    public class SpawnPositionPacket : IPacket
    {
        private MStream Stream;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public PacketList Id => PacketList.SPAWN_POSITION;
        public int PacketLength => 13;
        public IEnumerable<byte> Raw => Stream.Array;

        public SpawnPositionPacket(int x, int y, int z)
        {
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(x);
            Stream.Write(y);
            Stream.Write(z);
        }
    }
}
