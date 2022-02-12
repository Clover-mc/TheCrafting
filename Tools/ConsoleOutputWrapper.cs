using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Minecraft.Tools
{
    internal class ConsoleOutputWrapper : TextWriter
    {
        public static readonly StreamWriter Writer = new StreamWriter(Console.OpenStandardOutput());

        public override void Write(char value) { Write(value.ToString()); }
        public override void Write(bool value) { Write(value.ToString()); }
        public override void Write(char[] buffer, int index, int count) { Write(buffer.Skip(index).Take(count).ToString()); }
        public override void Write(char[]? buffer) { if (buffer is not null) Write(buffer, 0, buffer.Length); }
        public override void Write(decimal value) { Write(value.ToString()); }
        public override void Write(double value) { Write(value.ToString()); }
        public override void Write(float value) { Write(value.ToString()); }
        public override void Write(int value) { Write(value.ToString()); }
        public override void Write(long value) { Write(value.ToString()); }
        public override void Write(uint value) { Write(value.ToString()); }
        public override void Write(ulong value) { Write(value.ToString()); }

        public override void Write(string? value)
        {
            Write(value, ConsoleOutputLevel.LogLevel.INFO);
        }

        public static void Write(ConsoleColor color, string? value)
        {
            Write(color, value, ConsoleOutputLevel.LogLevel.INFO);
        }

        public static void Write(string? value, ConsoleOutputLevel.LogLevel level)
        {
            Writer.AutoFlush = true;

            EffectiveTools.ClearLineAndGoTo(Console.CursorTop);

            if (value is not null)
            {
                string message = "[" + EffectiveTools.GetTimeStamp() + "] [" + EffectiveTools.GetThreadName(Thread.CurrentThread) + "/" + ConsoleOutputLevel.GetName(level) + "]: " + value;
                MinecraftServer.Files.WriteToLog(message);
                Writer.WriteLine(message);
            }

            MinecraftServer.ConsoleInputHandler.UpdateInput(Writer);
        }

        public static void Write(ConsoleColor color, string? value, ConsoleOutputLevel.LogLevel level)
        {
            Writer.AutoFlush = true;

            EffectiveTools.ClearLineAndGoTo(Console.CursorTop);

            if (value is not null)
            {
                ConsoleColor before = Console.ForegroundColor;
                Console.ForegroundColor = color;

                string message = "[" + EffectiveTools.GetTimeStamp() + "] [" + EffectiveTools.GetThreadName(Thread.CurrentThread) + "/" + ConsoleOutputLevel.GetName(level) + "]: " + value;
                MinecraftServer.Files.WriteToLog(message);
                Writer.WriteLine(message);

                Console.ForegroundColor = before;
            }

            MinecraftServer.ConsoleInputHandler.UpdateInput(Writer);
        }

        public static void WriteError(Exception e)
        {
            Write(ConsoleColor.Red, e.ToString(), ConsoleOutputLevel.LogLevel.ERROR);
        }

        public override void WriteLine(string? value)
        {
            WriteLine(value, ConsoleOutputLevel.LogLevel.INFO);
        }

        public static void WriteLine(string? value, ConsoleOutputLevel.LogLevel level)
        {
            Writer.AutoFlush = true;

            EffectiveTools.ClearLineAndGoTo(Console.CursorTop);

            if (value is not null)
            {
                string message = "[" + EffectiveTools.GetTimeStamp() + "] [" + EffectiveTools.GetThreadName(Thread.CurrentThread) + "/" + ConsoleOutputLevel.GetName(level) + "]: " + value;
                MinecraftServer.Files.WriteToLog(message);
                Writer.WriteLine(message);
            }

            MinecraftServer.ConsoleInputHandler.UpdateInput(Writer);
        }

        public static void WriteLine(ConsoleColor color, string? value, ConsoleOutputLevel.LogLevel level)
        {
            Writer.AutoFlush = true;

            EffectiveTools.ClearLineAndGoTo(Console.CursorTop);

            if (value is not null)
            {
                ConsoleColor before = Console.ForegroundColor;
                Console.ForegroundColor = color;

                string message = "[" + EffectiveTools.GetTimeStamp() + "] [" + EffectiveTools.GetThreadName(Thread.CurrentThread) + "/" + ConsoleOutputLevel.GetName(level) + "]: " + value;
                MinecraftServer.Files.WriteToLog(message);
                Writer.WriteLine(message);

                Console.ForegroundColor = before;
            }

            MinecraftServer.ConsoleInputHandler.UpdateInput(Writer);
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        internal static void Initialize()
        {
            Writer.AutoFlush = true;
            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}
