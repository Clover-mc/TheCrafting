using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Minecraft.Tools
{
    public class MStream
    {
        private readonly MemoryStream Stream;
        public MStream()
        {
            Stream = new MemoryStream();
        }

        public static MStream From(byte[] array)
        {
            MStream stream = new MStream();

            stream.Write(array, 0, array.Length);
            stream.SetPosition(0);

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
            Write(data);
        }

        public void Write(sbyte data)
        {
            Write((byte)(data < 0 ? 0x100 + data : data));
        }

        public void Write(short data)
        {
            Write(BitConverter.GetBytes(data));
        }

        public void Write(ushort data)
        {
            Write(BitConverter.GetBytes(data));
        }

        public void Write(int data)
        {
            Write(BitConverter.GetBytes(data));
        }

        public void Write(long data)
        {
            Write(BitConverter.GetBytes(data));
        }

        public void Write(float data)
        {
            Write(BitConverter.GetBytes(data));
        }

        public void Write(double data)
        {
            Write(BitConverter.GetBytes(data));
        }

        public void Write(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            int length = data.Length;

            Write(new VarInt(length));
            Write(bytes);
        }

        public void WriteStringRaw(string data)
        {
            byte[] b = Encoding.UTF8.GetBytes(data);

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

        public void Write(byte[] buffer)
        {
            Stream.Write(buffer, 0, buffer.Length);
        }

        public void WriteDoublePacked(double d)
        {
            Write((int)(d * 32.0));
        }

        public void Write(VarInt varInt)
        {
            Write(varInt.ToPackedArray());
        }

        public void Write(Guid guid)
        {
            Write(guid.ToByteArray());
        }

        public byte Read()
        {
            return (byte)Stream.ReadByte();
        }

        public byte ReadByte()
        {
            return Read();
        }

        public short ReadShort()
        {
            byte[] short_ = new byte[2] { ReadByte(), ReadByte() };
            return BitConverter.ToInt16(short_.Reverse().ToArray(), 0);
        }

        public int ReadInt()
        {
            byte[] int_ = new byte[4] { ReadByte(), ReadByte(), ReadByte(), ReadByte() };
            return BitConverter.ToInt32(int_.Reverse().ToArray(), 0);
        }

        public byte[] ReadArray(int length)
        {
            byte[] array = new byte[length];
            Stream.Read(array, 0, length);
            return array;
        }

        public string ReadString()
        {
            int length = ReadVarInt().ToInt();
            return Encoding.UTF8.GetString(ReadArray(length));
        }

        public VarInt ReadVarInt()
        {
            VarInt varInt = VarInt.From(Stream.ToArray().Skip((int)Stream.Position).Take(3).ToArray());
            SetPosition(GetPosition() + varInt.ToPackedArray().Length);
            return varInt;
        }

        public Guid ReadGuid()
        {
            byte[] array = ReadArray(16);
            return new Guid(array);
        }

        public void PrefixWith(byte[] array)
        {
            byte[] original = Stream.ToArray();
            SetPosition(0);
            Write(array);
            Write(original);
        }

        public long GetPosition()
        {
            return Stream.Position;
        }

        public void SetPosition(long position)
        {
           Stream.Position = position;
        }
    }
}
