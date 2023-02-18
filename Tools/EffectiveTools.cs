using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Minecraft.Tools
{
    public static class EffectiveTools
    {
        public static readonly CultureInfo DefaultCulture = new CultureInfo("en-EN", true);

        public static string GetThreadName(Thread thread)
        {
            return thread.Name ?? "Unknown Thread";
        }

        public static string GetTimeStamp(DateTime time)
        {
            string hour = time.Hour.ToString(),
                minute = time.Minute.ToString(),
                second = time.Second.ToString();

            return (hour.Length > 1 ? hour : '0' + hour) + ':' + (minute.Length > 1 ? minute : '0' + minute) + ':' + (second.Length > 1 ? second : '0' + second);
        }

        public static string GetFullTimeStamp(DateTime time)
        {
            string hour = AppendStringBefore(time.Hour.ToString(DefaultCulture), 2);
            string minute = AppendStringBefore(time.Minute.ToString(DefaultCulture), 2);
            string second = AppendStringBefore(time.Second.ToString(DefaultCulture), 2);

            return GetDateStamp(time) + "-" + hour + "." + minute + "." + second;
        }
        public static string GetTimeStamp() => GetTimeStamp(DateTime.Now);
        public static string GetFullTimeStamp() => GetFullTimeStamp(DateTime.Now);

        public static string GetDateStamp(DateTime time)
        {
            string month = AppendStringBefore(time.Month.ToString(DefaultCulture), 2);
            string day = AppendStringBefore(time.Day.ToString(DefaultCulture), 2);
            string year = AppendStringBefore(time.Year.ToString(DefaultCulture), 4);

            return year + "." + month + "." + day;
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
            ConsoleWrapper.ConsoleWriter.Writer.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, line);
        }

        public static void ClearLine(int line)
        {
            int top = Console.CursorTop, left = Console.CursorLeft;

            Console.SetCursorPosition(0, line);
            ConsoleWrapper.ConsoleWriter.Writer.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(left, top);
        }

        public static byte[] AppendByteArray(byte[] array, int length)
        {
            if (array.Length >= length) return array;
            byte[] result = new byte[length];
            array.CopyTo(result, length - array.Length - 1);
            return result;
        }

        public static long GetUnixTimestamp()
        {
            return (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds;
        }
        public static int GetUnixSecondsTimestamp()
        {
            return (int)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
        }

        public static void PadStream(Stream stream, int pad)
        {
            long toPad = pad - (stream.Length % pad);
            if (toPad > 0)
                for (long i = 0; i < toPad; i++)
                    stream.WriteByte(0);
        }

        public static int ReadInt(Stream stream, bool asBigEndian = false)
        {
            byte[] arr = new byte[4] { (byte)stream.ReadByte(), (byte)stream.ReadByte(), (byte)stream.ReadByte(), (byte)stream.ReadByte() };
            if (asBigEndian) arr = arr.Reverse().ToArray();
            return BitConverter.ToInt32(arr);
        }
        public static int ReadInt24(Stream stream, bool asBigEndian = false)
        {
            byte[] arr = new byte[4] { 0, (byte)stream.ReadByte(), (byte)stream.ReadByte(), (byte)stream.ReadByte() };
            if (asBigEndian) arr = arr.Reverse().ToArray();
            return BitConverter.ToInt32(arr);
        }

        public static CultureInfo GetDefaultCultureInfo()
        {
            return DefaultCulture;
        }

        public static string ToLower(string str)
        {
            return str.ToLower(GetDefaultCultureInfo());
        }
        public static string ToUpper(string str)
        {
            return str.ToUpper(GetDefaultCultureInfo());
        }
        /*public static string ToCapital(string str)
        {
            return ToLower(str).ReplaceAt(0, char.ToUpper(str[0], GetDefaultCultureInfo())).Replace();
        }*/

        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
            if (input.Length == 0) return input;

            Span<char> chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        public static string GetFullClassNameOfCaller(int skipFrames = 2)
        {
            string fullName;
            Type declaringType;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName;
        }

        public static string GetClassNameOfCaller(int skipFrames = 2)
        {
            string fullName;
            Type declaringType;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.Name;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName;
        }

        public static double GetDistance3D(Location l1, Location l2)
        {
            double x = Math.Pow(l2.X - l1.X, 2);
            double y = Math.Pow(l2.Y - l1.Y, 2);
            double z = Math.Pow(l2.Z - l1.Z, 2);

            return Math.Sqrt(x + y + z);
        }
    }
}
