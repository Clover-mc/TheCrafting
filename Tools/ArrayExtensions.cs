using System;
using System.Linq;

namespace Minecraft.Tools;

public static class ArrayExtensions
{
    public static T[] ToBigEndian<T>(this T[] array)
    {
        if (BitConverter.IsLittleEndian)
        {
            return array.Reverse().ToArray();
        }

        return array;
    }
}
