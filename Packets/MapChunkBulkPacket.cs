using System.IO.Compression;

namespace Minecraft.Packets
{
    public class MapChunkBulkPacket : OutgoingPacket
    {
        private MStream Stream;
        private DeflateStream Deflate;

        public MapChunkBulkPacket(short chunk_column_count, bool send_sky_light, byte[] data)
        {
            byte[] new_data;
            using (MemoryStream ms = new MemoryStream())
            {
                Deflate = new DeflateStream(ms, CompressionLevel.SmallestSize);
                Deflate.Write(data, 0, data.Length);
                new_data = ms.ToArray();
            }
            Stream = new MStream();
            Stream.WriteByte(0x38);
            Stream.Write(chunk_column_count);
            Stream.Write(new_data.Length);
            Stream.Write(send_sky_light);
            Stream.Write(new_data, 0, new_data.Length);
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    Stream.Write(x);
                    Stream.Write(y);
                    Stream.Write((ushort)15);
                    Stream.Write((ushort)0);
                    
                }
            }
        }

        public override byte GetId()
        {
            return 0x38;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
