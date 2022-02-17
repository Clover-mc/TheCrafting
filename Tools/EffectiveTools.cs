using System;
using System.Threading;

namespace Minecraft.Tools
{
    public static class EffectiveTools
    {
        public static string GetThreadName(Thread thread)
        {
            return thread.Name is null ? "Unknown Thread" : thread.Name;
        }

        public static string GetTimeStamp()
        {
            byte hour = (byte)DateTime.Now.Hour,
                minute = (byte)DateTime.Now.Minute,
                second = (byte)DateTime.Now.Second;

            return (hour > 9 ? hour : "0" + hour) + ":" + (minute > 9 ? minute : "0" + minute) + ":" + (second > 9 ? second : "0" + second);
        }

        public static string GetFullTimeStamp()
        {
            string hour = AppendStringBefore(DateTime.Now.Hour.ToString(), 2);
            string minute = AppendStringBefore(DateTime.Now.Minute.ToString(), 2);
            string second = AppendStringBefore(DateTime.Now.Second.ToString(), 2);
            string month = AppendStringBefore(DateTime.Now.Month.ToString(), 2);
            string day = AppendStringBefore(DateTime.Now.Day.ToString(), 2);
            string year = AppendStringBefore(DateTime.Now.Year.ToString(), 4);

            return year + "." + month + "." + day + "-" + hour + "." + minute + "." + second;
        }

        public static string GetFullTimeStamp(DateTime time)
        {
            string hour = AppendStringBefore(time.Hour.ToString(), 2);
            string minute = AppendStringBefore(time.Minute.ToString(), 2);
            string second = AppendStringBefore(time.Second.ToString(), 2);
            string month = AppendStringBefore(time.Month.ToString(), 2);
            string day = AppendStringBefore(time.Day.ToString(), 2);
            string year = AppendStringBefore(time.Year.ToString(), 4);

            return year + "." + month + "." + day + "-" + hour + "." + minute + "." + second;
        }

        public static string GetShortTimeStamp(DateTime time)
        {
            string month = AppendStringBefore(time.Month.ToString(), 2);
            string day = AppendStringBefore(time.Day.ToString(), 2);
            string year = AppendStringBefore(time.Year.ToString(), 4);

            return year + "-" + month + "-" + day;
        }

        public static string AppendString(string str, int length)
        {
            while (str.Length < length) str += '0';
            return str;
        }

        public static string AppendStringBefore(string str, int length)
        {
            while (str.Length < length) str = "0" + str;
            return str;
        }

        public static void ClearLineAndGoTo(int line)
        {
            Console.SetCursorPosition(0, line);
            ConsoleOutputWrapper.GetWriter().Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, line);
        }

        public static void ClearLine(int line)
        {
            int top = Console.CursorTop, left = Console.CursorLeft;

            Console.SetCursorPosition(0, line);
            ConsoleOutputWrapper.GetWriter().Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(left, top);
        }

        public static byte[] AppendByteArray(byte[] array, int length)
        {
            if (array.Length >= length) return array;
            byte[] result = new byte[length];
            array.CopyTo(result, length - array.Length - 1);
            return result;
        }

        public static byte[] PackPacket(MStream stream)
        {
            VarInt length = new VarInt(stream.GetArray().Length);
            byte[] result = new byte[length.ToInt() + length.ToPackedArray().Length];

            length.ToPackedArray().CopyTo(result, 0);
            stream.GetArray().CopyTo(result, length.ToPackedArray().Length);

            return result;
        }
    }
}
