using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Minecraft.Commands;

namespace Minecraft.Tools
{
    public class ConsoleWrapper : TextReader
    {
        public class LogLevel : Enumeration
        {
            public static readonly LogLevel INFO    = new LogLevel(0, "INFO");
            public static readonly LogLevel WARNING = new LogLevel(1, "WARNING");
            public static readonly LogLevel ERROR   = new LogLevel(2, "ERROR");

            public LogLevel(int id, string value) : base(id, value) { }
        }
        public bool InputEnabled { get; private set; }
        public string Input { get; private set; }
        public ConsoleWriter Writer { get; private set; }

        private readonly MinecraftServer Server;
        private bool DoNewLine;

        internal ConsoleWrapper(MinecraftServer server)
        {
            Server = server;
            Writer = new ConsoleWriter(server, this);
            Input = "";
            DoNewLine = true;
        }

        public class ConsoleWriter : TextWriter
        {
            public static readonly StreamWriter Writer = new StreamWriter(Console.OpenStandardOutput());
            private static MinecraftServer server;
            private static ConsoleWrapper input;

            internal ConsoleWriter(MinecraftServer server, ConsoleWrapper input)
            {
                ConsoleWriter.server = server;
                ConsoleWriter.input = input;
                Writer.AutoFlush = true;
                Console.OutputEncoding = Encoding.UTF8;
            }

            public override void Write(char value) => Write(value.ToString());
            public override void Write(bool value) => Write(value.ToString());
            public override void Write(char[] buffer, int index, int count) => Write(buffer.Skip(index).Take(count).ToString());
            public override void Write(char[]? buffer) { if (buffer is not null) Write(buffer, 0, buffer.Length); }
            public override void Write(decimal value) => Write(value.ToString());
            public override void Write(double value) => Write(value.ToString());
            public override void Write(float value) => Write(value.ToString());
            public override void Write(int value) => Write(value.ToString());
            public override void Write(long value) => Write(value.ToString());
            public override void Write(uint value) => Write(value.ToString());
            public override void Write(ulong value) => Write(value.ToString());
            public override void Write(string? value) => Write(value ?? "", LogLevel.INFO);

            public static void Write(ConsoleColor color, string value) => Write(color, value, LogLevel.INFO);
            public static void Write(string value, LogLevel level) => Write(Console.ForegroundColor, value, level);
            public static void Write(ConsoleColor color, string value, LogLevel level)
            {
                if (server.ConWrapper.DoNewLine) EffectiveTools.ClearLineAndGoTo(Console.CursorTop);

                if (value is not null)
                {
                    if (color != Console.ForegroundColor) Console.ForegroundColor = color;

                    string message = value;
                    if (server.ConWrapper.DoNewLine)
                    {
                        message = "[" + EffectiveTools.GetTimeStamp() + "] [" + EffectiveTools.GetThreadName(Thread.CurrentThread) + "/" + level.Name + "]: " + message;
                        server.ConWrapper.DoNewLine = false;
                    }

                    server.Files.WriteToLogRaw(message);
                    Writer.Write(message);

                    if (color != Console.ForegroundColor) ResetColor();
                }

                input.UpdateInput(Writer);
            }

            public static void AllocLine()
            {
                if (server.ConWrapper.DoNewLine) EffectiveTools.ClearLineAndGoTo(Console.CursorTop);

                if (!server.ConWrapper.DoNewLine) Console.Write('\n');

                server.ConWrapper.DoNewLine = false;

                string message = "[" + EffectiveTools.GetTimeStamp() + "] [" + EffectiveTools.GetThreadName(Thread.CurrentThread) + "/" + LogLevel.INFO.Name + "]: ";
                server.Files.WriteToLogRaw(message);
                Writer.Write(message);

                input.UpdateInput(Writer);
            }

            public static void WriteError(Exception e) => WriteError(e.ToString());
            public static void WriteError(string value) => WriteLine(ConsoleColor.Red, value, LogLevel.ERROR);

            public override void WriteLine() => base.WriteLine();
            public override void WriteLine(string? value) => WriteLine(value ?? "", LogLevel.INFO);
            public static void WriteLine(string value, LogLevel level) => WriteLine(Console.ForegroundColor, value, level);
            public static void WriteLine(ConsoleColor color, string value, LogLevel level)
            {
                if (server.ConWrapper.DoNewLine) EffectiveTools.ClearLineAndGoTo(Console.CursorTop);

                if (value is not null)
                {
                    if (color != Console.ForegroundColor) Console.ForegroundColor = color;

                    if (!server.ConWrapper.DoNewLine)
                    {
                        server.ConWrapper.DoNewLine = true;
                        Writer.WriteLine();
                    }

                    string message = "[" + EffectiveTools.GetTimeStamp() + "] [" + EffectiveTools.GetThreadName(Thread.CurrentThread) + "/" + level.Name + "]: " + value;
                    server.Files.WriteToLog(message);
                    Writer.WriteLine(message);

                    ResetColor();
                }

                input.UpdateInput(Writer);
            }

            public static void ResetColor()
            {
                int top = Console.CursorTop, left = Console.CursorLeft;

                Console.ResetColor();
                Writer.WriteLine();

                Console.CursorTop = top; Console.CursorLeft = left;
            }

            public override Encoding Encoding { get => Encoding.UTF8; }
        }

        public static bool IsPrintableChar(char character)
        {
            return char.IsLetterOrDigit(character) || char.IsPunctuation(character) || char.IsSymbol(character) || character == ' ';
        }

        public static void ClearLastLine(int offset = 0, TextWriter writer = null)
        {
            if (writer is null) writer = Console.Out;

            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(offset, Console.CursorTop);
            writer.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(offset, currentLineCursor);
        }

        internal void UpdateInput(TextWriter writer = null)
        {
            int leftPos = Console.CursorLeft;

            if (!InputEnabled) return;
            if (writer is null) writer = Console.Out;
            if (!DoNewLine) writer.WriteLine("");

            ClearLastLine(0, writer);
            writer.Write('>' + (Input.Length >= Console.WindowWidth - 1 ? new string(Input.Skip(Input.Length - Console.WindowWidth + 1).ToArray()) : Input));

            if (!DoNewLine)
            {
                Console.CursorTop--;
                Console.CursorLeft = leftPos;
            }
        }

        internal void DisableInput() => InputEnabled = false;

        internal void EnableInput()
        {
            InputEnabled = true;
            Task.Run(() =>
            {
                Thread.CurrentThread.Name = "Console Input Thread";
                while (InputEnabled)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        char keyChar = key.KeyChar;

                        switch (key.Key)
                        {
                            case ConsoleKey.Escape:
                                Environment.Exit(0);
                                break;
                            case ConsoleKey.Tab:
                                //RequestTabComplete();
                                break;
                            case ConsoleKey.Backspace:
                                if (Input.Length == 0) break;
                                Input = Input.Remove(Input.Length - 1);
                                Console.Write(null as string);
                                break;
                            case ConsoleKey.Enter:
                                if (Input == "") break;

                                try { Server.Commands.TryParse("/" + Input, Server.ConsolePlayer); }
                                catch (Exception e){ ConsoleWriter.WriteError(e); }

                                Input = "";
                                Console.Write(null as string);
                                break;
                        }

                        if (!IsPrintableChar(key.KeyChar)) continue;

                        Input += key.KeyChar;

                        Console.Write(null as string);
                    }
                }
            });
        }
    }
}