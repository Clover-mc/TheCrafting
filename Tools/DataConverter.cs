using System;

namespace Minecraft.Tools
{
    public static class DataConverter
    {
        public static byte[] ToByteArray(bool data)    { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(byte data)    { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(sbyte data)   { return BitConverter.GetBytes((byte)(data < 0 ? 0x100 + data : data)); }
        public static byte[] ToByteArray(char data)    { return BitConverter.GetBytes(data); }
        // public static byte[] ToByteArray(decimal data) { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(double data)  { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(float data)   { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(int data)     { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(uint data)    { return BitConverter.GetBytes(data); }
        // public static byte[] ToByteArray(nint data)
        // {
        //     if ((long)IntPtr.MaxValue == long.MaxValue) return BitConverter.GetBytes(data);
        //     else return BitConverter.GetBytes((int)data);
        // }
        // public static byte[] ToByteArray(nuint data)
        // {
        //     if ((ulong)UIntPtr.MaxValue == ulong.MaxValue) return BitConverter.GetBytes(data);
        //     else return BitConverter.GetBytes((uint)data);
        // }
        public static byte[] ToByteArray(long data)    { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(ulong data)   { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(short data)   { return BitConverter.GetBytes(data); }
        public static byte[] ToByteArray(ushort data)  { return BitConverter.GetBytes(data); }

        public static byte ToByte(sbyte data)          { return (byte)(data < 0 ? 0x100 + data : data); }
        public static sbyte ToSbyte(byte data)         { return (sbyte)(data > 0x7F ? data - 0x100 : data); }
    }
}
