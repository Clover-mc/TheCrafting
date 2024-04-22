using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minecraft.Tools;

public class MStream
{
    public long Position
    {
        get => Stream.Position;
        set => Stream.Position = value;
    }

    public byte[] Array => Stream.ToArray();
    
    public MemoryStream Stream { get; }

    public MStream()
    {
        Stream = new();
    }
    
    public MStream(IEnumerable<byte> data)
        : this(data.ToArray()) { }

    public MStream(byte[] data)
    {
        Stream = new(data, 0, data.Length)
        {
            Position = 0
        };
    }

    public MStream Write(byte data) => WriteByte(data);
    public MStream Write(sbyte data) => Write(unchecked((byte)data));
    public MStream Write(short data) => Write(unchecked((byte)(data >> 8))).Write(unchecked((byte)data));
    public MStream Write(ushort data) => Write(unchecked((byte)(data >> 8))).Write(unchecked((byte)data));
    public MStream Write(bool data) => Write((byte)(data ? 1 : 0));
    public MStream WriteDoublePacked(double d) => Write((int)(d * 32.0));
    public MStream Write(float data) => Write(BitConverter.SingleToInt32Bits(data));
    public MStream Write(double data) => Write(BitConverter.DoubleToInt64Bits(data));

    public MStream WriteByte(byte data)
    {
        Stream.WriteByte(data);
        return this;
    }

    public MStream Write(int data)
    {
        Write(unchecked((byte)(data >> 24)));
        Write(unchecked((byte)(data >> 16)));
        Write(unchecked((byte)(data >> 8)));
        Write(unchecked((byte)data));
        return this;
    }

    public MStream Write(long data)
    {
        Write(unchecked((byte)(data >> 56)));
        Write(unchecked((byte)(data >> 48)));
        Write(unchecked((byte)(data >> 40)));
        Write(unchecked((byte)(data >> 32)));
        Write(unchecked((byte)(data >> 24)));
        Write(unchecked((byte)(data >> 16)));
        Write(unchecked((byte)(data >> 8)));
        Write(unchecked((byte)data));
        return this;
    }

    public MStream Write(string data)
    {
        byte[] b = Encoding.BigEndianUnicode.GetBytes(data);
        int length = data.Length;

        Write((short)length);
        return Write(b, 0, b.Length);
    }

    public MStream Write8(string data)
    {
        byte[] b = Encoding.UTF8.GetBytes(data);
        Write((short)b.Length);
        return Write(b, 0, b.Length);
    }

    public MStream Write(int[] array)
    {
        int byteDim = array.Length * sizeof(int);
        byte[] bytes = new byte[byteDim];
        Buffer.BlockCopy(array, 0, bytes, 0, byteDim);
        return Write(bytes, 0, byteDim);
    }

    public MStream Write(byte[] buffer, int offset, int count)
    {
        Stream.Write(buffer, offset, count);
        return this;
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
        short length = ReadShort();
        return Encoding.BigEndianUnicode.GetString(ReadArray(length * 2)); // Every character using 2 bytes to store itself
    }

    public double ReadDouble()
    {
        return BitConverter.ToDouble(ReadArray(8), 0);
    }

    public float ReadFloat()
    {
        return BitConverter.ToSingle(ReadArray(4), 0);
    }

    public bool ReadBoolean()
    {
        return ReadByte() != 0;
    }
}
