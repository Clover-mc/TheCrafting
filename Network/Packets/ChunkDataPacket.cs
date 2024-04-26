using System.Collections.Generic;
using Minecraft.Tools;

namespace Minecraft.Network.Packets
{
    public class ChunkDataPacket : IPacket
    {
        private MStream Stream;

        public PacketList Id => PacketList.ChunkData;
        private int _packetLength;
        public int PacketLength => _packetLength;
        public IEnumerable<byte> Raw => Stream.Array;

        public ChunkDataPacket(int x, int z)
        {
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(x);
            Stream.Write(z);
            Stream.Write(true);
            Stream.Write((ushort)15);
            Stream.Write((ushort)0);

            byte[] general = new byte[12544];
            for (int i = 0; i < 4096; i++)
            {
                if (i < 1024) // 8192
                {
                    if (i < 256)
                    {
                        general[i + 12288] = 0x02;
                    }
                    general[i + 6144] = 0xFF;
                }
                general[i] = 0x01;
            }

            byte[] compressed = ZLib.Compress(general);
            Stream.Write(compressed.Length);
            Stream.Write(compressed, 0, compressed.Length);
            _packetLength = 18 + compressed.Length;
        }
    }
}
