using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Network.Packets
{
    public class SpawnPositionPacket : IPacket
    {
        private MStream Stream;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public PacketList Id => PacketList.SpawnPosition;
        public int PacketLength => 13;
        public IEnumerable<byte> Raw => Stream.Array;

        public SpawnPositionPacket(int x, int y, int z)
        {
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(X = x);
            Stream.Write(Y = y);
            Stream.Write(Z = z);
        }
    }
}
