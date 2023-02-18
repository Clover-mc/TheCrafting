using System.IO;
using System.IO.Compression;

namespace Minecraft.Tools
{
    public static class ZLib
    {
        public static byte[] Compress(byte[] data, CompressionLevel level = CompressionLevel.Optimal)
        {
            MemoryStream memory = new MemoryStream();
            using (DeflateStream stream = new DeflateStream(memory, level))
            {
                stream.Write(data, 0, data.Length);
            }
            return memory.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream stream = new DeflateStream(output, CompressionMode.Decompress))
            {
                input.CopyTo(output);
            }
            return output.ToArray();
        }
    }
}
