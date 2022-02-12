using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using Minecraft.Tools;

namespace Minecraft.Command
{
    internal class CommandHandler
    {
        public static string nick = "";
        public static void TryParse(string command)
        {
            if (command == null || command.Length == 0) return;
            if (command[0] == '/')
            {
                string label = command.Substring(1, command.Contains(' ') ? command.IndexOf(' ') - 1 : command.Length - 1);

                switch(label)
                {
                    case "exit":
                        EffectiveTools.ShutdownServer();
                        break;
                    case "help":
                        Console.Write("== Help");
                        Console.Write(" | exit - shutdown server");
                        Console.Write(" | help - show this");
                        Console.Write(" | clear - clear console");
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    case "mstest":
                        MStream stream = new MStream();
                        stream.WriteStringRaw("LOX");
                        Console.Write(BitConverter.ToString(stream.GetArray()));
                        stream.WriteStringRaw("HOW");
                        Console.Write(BitConverter.ToString(stream.GetArray()));
                        Console.Write(stream.GetArray()[0] == 'L');
                        Console.Write(stream.GetArray()[0] == 'H');
                        break;
                    case "serialtext":
                        Console.Write("False True");
                        Console.Write(false.ToString() + " " + true.ToString());
                        break;
                    case "serialtexttest":
                        byte[] bytes;
                        //byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(new Chat.Builder.Text.JsonText("Hello!", "yellow", true));
                        //Console.Write(BitConverter.ToString(bytes));
                        //Console.Write(Encoding.UTF8.GetString(bytes));
                        break;
                    case "SerializeTextV2Test":
                        //Console.Write(new Chat.Builder.Text.JsonText("Wtf u mean?", null, null, true).ToJson());
                        break;
                    case "dyntest":
                        dynamic sheesh = new {
                            text = "Dab",
                            bold = true,
                            extra = new int[5]
                        };
                        
                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        Console.Write(BitConverter.ToString(bytes));
                        Console.Write(Encoding.UTF8.GetString(bytes));
                        break;
                    case "simpletryoverflow":
                        byte[] mybytes = new byte[5];
                        byte[] badbytes = new byte[8] { 0, 5, 1, 73, 23, 72, 64, 23 };
                        badbytes.CopyTo(mybytes, 0);
                        break;
                    case "newdyn":
                        sheesh = new System.Dynamic.ExpandoObject();

                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        Console.Write(BitConverter.ToString(bytes));
                        Console.Write(Encoding.UTF8.GetString(bytes));

                        sheesh.text = "bruh";
                        sheesh.italic = "true";
                        sheesh.extra = new List<Chat.Builder.TextComponent>();

                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        Console.Write(BitConverter.ToString(bytes));
                        Console.Write(Encoding.UTF8.GetString(bytes));

                        sheesh.extra.Add(new Chat.Builder.ItalicText("Booba"));

                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        Console.Write(BitConverter.ToString(bytes));
                        Console.Write(Encoding.UTF8.GetString(bytes));
                        break;
                    case "eu":
                        nick = command[4..];
                        Console.Write('"' + nick + '"');
                        break;
                    case "du":
                        nick = "";
                        break;
                    default:
                        ConsoleOutputWrapper.Write("Unknown command. Type \"/help\" for help.", ConsoleOutputLevel.LogLevel.ERROR);
                        break;
                }
            }
        }
    }
}
