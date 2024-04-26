using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Network.Packets
{
    public class BlockChangePacket : IPacket
    {
        private MStream Stream;

        public PacketList Id => PacketList.BlockChange;
        public int PacketLength => 13;
        public IEnumerable<byte> Raw => Stream.Array;

        public BlockChangePacket(int x, byte y, int z, short id, byte meta)
        {
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(x);
            Stream.Write(y);
            Stream.Write(z);
            Stream.Write(id);
            Stream.Write(meta);
        }
    }
}
