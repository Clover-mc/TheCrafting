using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using Minecraft.Entities;
using Minecraft.Packets;
using Minecraft.Tools;

namespace Minecraft.Commands
{
    public class CommandsHandler
    {
        private Dictionary<string, ICommand> Commands = new Dictionary<string, ICommand>();

        private static string undercoverNickname = "";

        internal CommandsHandler() { }

        public bool RegisterCommand(string label, ICommand handler)
        {
            return Commands.TryAdd(label, handler);
        }

        public bool UnregisterCommand(string label)
        {
            return Commands.Remove(label);
        }

        public void TryParse(string input, Player sender)
        {
            if (input == null || input.Length == 0 || input[0] != '/') return;

            string label = input.Substring(1, input.Contains(' ') ? input.IndexOf(' ') - 1 : input.Length - 1);

            if (Commands.TryGetValue(label, out ICommand? value))
                value.OnCommand(sender, label, input, Array.Empty<string>());
            else
            {
                switch (label)
                {
                    case "exit":
                        Environment.Exit(0);
                        break;
                    case "clear":
                        if (sender is ConsolePlayer) Console.Clear();
                        else sender.SendMessage("Wait, you are not a console, you cannot clear!");
                        break;
                    case "teleportme":
                        if (sender is ConsolePlayer)
                        {
                            sender.SendMessage("I cannot teleport console!");
                            break;
                        }
                        sender.Connection.SendPacket(new PlayerPositionPacket(0.5, 1024, 0.5, 0, false));
                        break;
                    case "chunk":
                        if (sender is ConsolePlayer)
                        {
                            sender.SendMessage("I cannot send chunk to console!");
                            break;
                        }
                        sender.Connection.SendPacket(new ChunkDataPacket(0, 0));
                        break;
                    case "blockpls":
                        if (sender is ConsolePlayer)
                        {
                            sender.SendMessage("I cannot add block to console's world!");
                            break;
                        }
                        sender.Connection.SendPacket(new BlockChangePacket(0, 128, 0, 1, 0));
                        break;
                    case "mstest":
                        MStream stream = new MStream();
                        stream.Write("LOX");
                        sender.SendMessage(BitConverter.ToString(stream.Array));
                        stream.Write("HOW");
                        sender.SendMessage(BitConverter.ToString(stream.Array));
                        sender.SendMessage((stream.Array[0] == 'L').ToString());
                        sender.SendMessage((stream.Array[0] == 'H').ToString());
                        break;
                    case "serialtext":
                        sender.SendMessage("False True");
                        sender.SendMessage(false.ToString() + " " + true.ToString());
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
                        dynamic sheesh = new
                        {
                            text = "Dab",
                            bold = true,
                            extra = new int[5]
                        };

                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        sender.SendMessage(BitConverter.ToString(bytes));
                        sender.SendMessage(Encoding.UTF8.GetString(bytes));
                        break;
                    case "simpletryoverflow":
                        byte[] mybytes = new byte[5];
                        byte[] badbytes = new byte[8] { 0, 5, 1, 73, 23, 72, 64, 23 };
                        badbytes.CopyTo(mybytes, 0);
                        break;
                    case "newdyn":
                        sheesh = new System.Dynamic.ExpandoObject();

                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        sender.SendMessage(BitConverter.ToString(bytes));
                        sender.SendMessage(Encoding.UTF8.GetString(bytes));

                        sheesh.text = "bruh";
                        sheesh.italic = "true";
                        //sheesh.extra = new List<Chat.Builder.TextComponent>();

                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        sender.SendMessage(BitConverter.ToString(bytes));
                        sender.SendMessage(Encoding.UTF8.GetString(bytes));

                        //sheesh.extra.Add(new Chat.Builder.ItalicText("Booba"));

                        bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
                        sender.SendMessage(BitConverter.ToString(bytes));
                        sender.SendMessage(Encoding.UTF8.GetString(bytes));
                        break;
                    case "eu":
                        undercoverNickname = input[4..];
                        sender.SendMessage('"' + undercoverNickname + '"');
                        break;
                    case "du":
                        undercoverNickname = "";
                        break;
                    case "contest":
                        Console.Write("AAA");
                        Console.Write("BBB");
                        Console.WriteLine("CCC");
                        Console.WriteLine("DDD");
                        break;
                    case "colortest":
                        sender.SendMessage(ChatColor.Green + "Green! " + ChatColor.Reset + ChatColor.Strikethrough + "Ohhh... " + ChatColor.Reset + ChatColor.Bold + " I am bold! " + ChatColor.Reset + "I am normal!");
                        sender.SendMessage(ChatColor.Red + "Red pill " + ChatColor.Reset + "or " + ChatColor.Blue + "Blue " + ChatColor.Reset + "one?");
                        break;
                    case "ismyground":
                        sender.SendMessage((sender.IsOnGround ? ChatColor.Green + "Yeah, True" : ChatColor.Red + "Uhhh, False") + ChatColor.Reset);
                        break;
                    case "dn":
                        if (sender is ConsolePlayer) sender.SendMessage(ChatColor.Red + "This command is only for players!");
                        else sender.DisplayName = input[4..];
                        break;
                    default:
                        sender.SendMessage("Unknown command. Type \"/help\" for help.");
                        break;
                }
            }
        }
    }
}
