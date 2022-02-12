namespace Minecraft.World
{
    public class BiomeData
    {
        private byte[]? Data;

        public BiomeData()
        {
            // Length = 16 * 16 = 256
            Data = null;
        }

        public void Fill()
        {
            if (Data is null)
            {
                Data = new byte[256];
            }
        }

        public void Unpack(byte[] buffer)
        {
            if (Data is null) Data = new byte[256];
            buffer.CopyTo(Data, 0);
        }

        public byte[] Pack()
        {
            if (Data is null) Data = new byte[256];
            return Data;
        }

        public byte Get(int x, int z)
        {
            if (Data is null) Data = new byte[256];
            return Data[x + z * 16];
        }

        public void Put(int x, int z, byte data)
        {
            if (Data is null) Data = new byte[256];
            Data[x + z * 16] = data;
        }
    }
}
