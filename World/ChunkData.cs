namespace Minecraft.World
{
    public class ChunkData
    {
        private const int Length = 4096; // 16^3 = 16 * 16 * 16
        private byte[]? Data;

        public ChunkData()
        {
            Data = null;
        }

        public void Fill()
        {
            if (Data is null)
            {
                Data = new byte[Length];
            }
        }

        public void Unpack(byte[] buffer)
        {
            if (Data is null) Data = new byte[Length];
            buffer.CopyTo(Data, 0);
        }

        public byte[] Pack()
        {
            if (Data is null) Data = new byte[Length];
            return Data;
        }

        public byte Get(int x, int y, int z)
        {
            if (Data is null) Data = new byte[Length];
            return Data[x + ((y * 16) + z) * 16];
        }

        public void Put(int x, int y, int z, byte data)
        {
            if (Data is null) Data = new byte[Length];
            Data[x + ((y * 16) + z) * 16] = data;
        }
    }
}
