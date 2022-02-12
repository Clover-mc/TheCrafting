namespace Minecraft.Tools
{
    public static class PacketPack
    {
        public static byte[] Pack(MStream stream)
        {
            VarInt length = new VarInt(stream.GetArray().Length);
            byte[] result = new byte[length.ToInt() + length.ToPackedArray().Length];

            length.ToPackedArray().CopyTo(result, 0);
            stream.GetArray().CopyTo(result, length.ToPackedArray().Length);

            return result;
        }
    }
}
