using System.IO;
using System.IO.Compression;

namespace Minecraft.Tools;

public static class ZLib
{
    public static byte[] Compress(byte[] data, CompressionLevel level = CompressionLevel.Optimal)
    {
        var memory = new MemoryStream();
        using (var stream = new DeflateStream(memory, level))
        {
            stream.Write(data, 0, data.Length);
        }
        return memory.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        var input = new MemoryStream(data);
        var output = new MemoryStream();
        using (var stream = new DeflateStream(output, CompressionMode.Decompress))
        {
            input.CopyTo(output);
        }
        return output.ToArray();
    }
}
