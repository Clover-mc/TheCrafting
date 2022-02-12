namespace Minecraft.World
{
    public class ChunkHalfData : ChunkData
    {
        private const int Length = 2048; // 16 * 16 * 8
        private byte[]? Data;

        public ChunkHalfData()
        {
            Data = null;
        }

        public new byte Get(int x, int y, int z)
        {
            if (Data is null) Data = new byte[Length];

            x /= 2;
            int r = x % 2;
            int i = x + ((y * 16) + z) * 16;

            if (r == 0)
            {
                return (byte)(Data[i] >> 4);
            }
            else
            {
                return (byte)(Data[i] & 0x0F);
            }
        }

        public new void Put(int x, int y, int z, byte data)
        {
            if (Data is null) Data = new byte[Length];

            x /= 2;
            int r = x % 2;
            int i = x + ((y * 16) + z) * 16;

            if (r == 0)
            {
                Data[i] = (byte)((Data[i] & 0x0F) | ((data & 0x0F) << 4));
            }
            else
            {
                Data[i] = (byte)((Data[i] & 0xF0) | (data & 0x0F));
            }
        }
    }
}
