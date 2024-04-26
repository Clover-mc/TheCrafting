using System;
using System.Collections.Generic;
using Minecraft.Tools;

namespace Minecraft.Network.Packets
{
    public class MapChunkBulkPacket : IPacket
    {
        private readonly MStream Stream;

        public short ChunkColumnCount { get; private set; }
        public bool SendSkyLight { get; private set; }

        public PacketList Id => PacketList.MapChunkBulk;
        public int PacketLength => -1;
        public IEnumerable<byte> Raw => Stream.Array;

        public MapChunkBulkPacket(short chunk_column_count, bool send_sky_light, byte[] data)
        {
            ChunkColumnCount = chunk_column_count;
            SendSkyLight = send_sky_light;

            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(ChunkColumnCount);
            Stream.Write(data.Length);
            Stream.Write(SendSkyLight);
            Stream.Write(data, 0, data.Length);
            for (int x = 0; x < Math.Sqrt(ChunkColumnCount); x++)
                for (int z = 0; z < Math.Sqrt(ChunkColumnCount); z++)
                {
                    Stream.Write(x);
                    Stream.Write(z);
                    Stream.Write((ushort)0);
                    Stream.Write((ushort)0);
                }
        }
    }
}
