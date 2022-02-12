using System;
using System.Linq;
using System.Text;

namespace Minecraft.Tools
{
    public class VarInt
    {
        private readonly int Source = 0;
        private readonly int Packed = 0;
        public VarInt(int integer)
        {
            Source = integer;
            Packed = BitConverter.ToInt32(EffectiveTools.AppendByteArray(ToPackedArray(), 4));
        }
        public int ToInt()
        {
            return Source;
        }

        public int ToPackedInt()
        {
            return Packed;
        }

        public byte[] ToPackedArray()
        {
            byte[] bytes = new byte[3];
            int value = ToInt();
            int i = 0;
            for (; i < 3; i++)
            {
                // Only the first 7 bits have data
                if ((value & 0b11111111111111111111111110000000) == 0)
                {
                    bytes[i] = (byte)value;
                    break;
                }

                bytes[i] = (byte)(value & 0b01111111 | 0b10000000);
                // Note: >>> means that the sign bit is shifted with the rest of the number rather than being left alone
                value >>= 7;
            }
            byte[] final = new byte[i + 1];
            bytes.Take(i + 1).ToArray().CopyTo(final, 0);
            return final;
        }

        public byte[] Read()
        {
            byte[] bytes = ToPackedArray();
            int value = 0;
            int bitOffset = 0;
            byte currentByte;
            do
            {
                if (bitOffset == 35) throw new ArgumentException("VarInt is too big");

                currentByte = bytes[bitOffset / 7];
                value |= (currentByte & 0b01111111) << bitOffset;

                bitOffset += 7;
            } while ((currentByte & 0b10000000) != 0);

            return BitConverter.GetBytes(value);
        }

        public byte[] Read(byte length)
        {
            byte[] result = new byte[length];
            Read().CopyTo(result, length);
            return result;
        }

        public static VarInt From(int varint)
        {
            return From(BitConverter.GetBytes(varint));
        }

        public static VarInt From(string varint)
        {
            return From(Encoding.UTF8.GetBytes(varint));
        }

        public static VarInt From(byte[] bytes, bool reversed = false)
        {
            int value = 0;
            int length = 0;
            byte currentByte;

            while (bytes.Length > length)
            {
                currentByte = bytes[length];
                value |= (currentByte & 0x7F) << (length * 7);

                length += 1;
                if (length > 5)
                {
                    throw new ArgumentException("VarInt is too big");
                }

                if ((currentByte & 0x80) != 0x80)
                {
                    break;
                }
            }
            if (reversed) value = BitConverter.ToInt32(ReverseArray(BitConverter.GetBytes(value)));
            return new VarInt(value);

            /*
            if (bytes.Length == 0) return new VarInt(0);
            int value = 0;
            int bitOffset = 0;
            byte currentByte;
            do
            {
                if (bitOffset == 35) throw new ArgumentException("VarInt is too big");

                currentByte = bytes[bitOffset / 7];
                //value |= (currentByte & 0b01111111) << bitOffset;
                value |= currentByte << (bitOffset / 7 * 8);

                bitOffset += 7;
            } while ((currentByte & 0b10000000) != 0);
            if (reversed) value = BitConverter.ToInt32(ReverseArray(BitConverter.GetBytes(value)));
            return new VarInt(value);*/
        }

        private static byte[] ReverseArray(byte[] arr)
        {
            byte[] new_arr = new byte[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                new_arr[arr.Length - i - 1] = arr[i];
            }

            return new_arr;
        }
    }
}
