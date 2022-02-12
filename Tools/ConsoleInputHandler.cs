using System;
using System.IO;
using System.Linq;
using System.Threading;
using Minecraft.Command;

namespace Minecraft.Tools
{
    public class ConsoleInputHandler
    {
        public bool Enabled { get; private set; } = false;
        private Thread? Thread;
        private string Input;

        internal ConsoleInputHandler()
        {
            Input = "";
            Thread = null;
        }

        internal void Enable()
        {
            Enabled = true;
            Thread = new Thread(() =>
            {
                Thread.CurrentThread.Name = "Console Input Thread";
                while (Enabled)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        char keyChar = key.KeyChar;

                        switch (key.Key)
                        {
                            case ConsoleKey.Escape:
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
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
                                //SendCommand(Input);
                                if (Input == "") break;
                                CommandHandler.TryParse("/" + Input);
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
            Thread.Start();
        }

        internal void Disable()
        {
            Enabled = false;
        }

        public static bool IsPrintableChar(char character)
        {
            return char.IsLetterOrDigit(character) || char.IsPunctuation(character) || char.IsSymbol(character) || character == ' ';
        }

        public static void ClearLastLine(int offset = 0, TextWriter? writer = null)
        {
            if (writer is null) writer = Console.Out;

            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(offset, Console.CursorTop);
            writer.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(offset, currentLineCursor);
        }

        internal void UpdateInput(TextWriter? writer = null)
        {
            if (!Enabled) return;
            if (writer is null) writer = Console.Out;
            ClearLastLine(0, writer);
            writer.Write('>' + (Input.Length >= Console.WindowWidth - 1 ? new string(Input.Skip(Input.Length - Console.WindowWidth + 1).ToArray()) : Input));
        }
    }
}
