using System.Text;

namespace Minecraft
{
    public class MStream
    {
        private MemoryStream Stream;
        public MStream()
        {
            Stream = new MemoryStream();
        }

        public static MStream From(byte[] array)
        {
            MStream stream = new MStream();

            stream.GetStream().Write(array, 0, array.Length);

            return stream;
        }

        public MemoryStream GetStream()
        {
            return Stream;
        }

        public byte[] GetArray()
        {
            return Stream.ToArray();
        }

        public void Write(byte data)
        {
            Stream.WriteByte(data);
        }

        public void WriteByte(byte data)
        {
            Stream.WriteByte(data);
        }

        public void Write(sbyte data)
        {
            Write(unchecked((byte)data));
        }

        public void Write(short data)
        {
            Write(unchecked((byte)(data >> 8)));
            Write(unchecked((byte)data));
        }

        public void Write(ushort data)
        {
            Write(unchecked((byte)(data >> 8)));
            Write(unchecked((byte)data));
        }

        public void Write(int data)
        {
            Write(unchecked((byte)(data >> 24)));
            Write(unchecked((byte)(data >> 16)));
            Write(unchecked((byte)(data >> 8)));
            Write(unchecked((byte)data));
        }

        public void Write(long data)
        {
            Write(unchecked((byte)(data >> 56)));
            Write(unchecked((byte)(data >> 48)));
            Write(unchecked((byte)(data >> 40)));
            Write(unchecked((byte)(data >> 32)));
            Write(unchecked((byte)(data >> 24)));
            Write(unchecked((byte)(data >> 16)));
            Write(unchecked((byte)(data >> 8)));
            Write(unchecked((byte)data));
        }

        public unsafe void Write(float data)
        {
            Write(*(int*)&data);
        }

        public unsafe void Write(double data)
        {
            Write(*(long*)&data);
        }

        public void Write(string data)
        {
            byte[] b = Encoding.BigEndianUnicode.GetBytes(data);
            int length = data.Length;

            Write((short)length);
            Write(b, 0, b.Length);
        }

        public void Write8(string data)
        {
            byte[] b = ASCIIEncoding.UTF8.GetBytes(data);
            Write((short)b.Length);
            Write(b, 0, b.Length);
        }

        public void Write(bool data)
        {
            Write((byte)(data ? 1 : 0));
        }

        public void Write(int[] array)
        {
            int byteDim = array.Length * sizeof(int);
            byte[] bytes = new byte[byteDim];
            Buffer.BlockCopy(array, 0, bytes, 0, byteDim);
            Stream.Write(bytes, 0, byteDim);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            Stream.Write(buffer, offset, count);
        }

        public void WriteDoublePacked(double d)
        {
            Write((int)(d * 32.0));
        }
    }
}
