using System.Text;
using System.Security.Cryptography;
using System;
using System.Linq;

namespace Minecraft.Tools
{
    public static class UUID
    {
        public readonly static MD5 Md5 = MD5.Create();

        public static Guid FromString(string value)
        {
            byte[] hash = Md5.ComputeHash(Encoding.UTF8.GetBytes(value)).Reverse().ToArray();
            Array.Reverse(hash);

            return FromByteArray(hash);
        }

        public static Guid FromByteArray(byte[] array)
        {
            array[6] &= 0x0f;  /* clear version        */
            array[6] |= 0x30;  /* set to version 3     */
            array[8] &= 0x3f;  /* clear variant        */
            array[8] |= 0x80;  /* set to IETF variant  */

            string source = BitConverter.ToString(array).Replace("-", "");
            string result = source[..8] + '-' + source.Substring(8, 4) + '-' +
                source.Substring(12, 4) + '-' + source.Substring(16, 4) + '-' + source.Substring(20, 12);

            return Guid.Parse(new ReadOnlySpan<char>(result.ToCharArray()));
        }
    }
}
