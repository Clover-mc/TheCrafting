using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using Minecraft.Entities;
using Minecraft.Packets;
using Minecraft.Tools;

namespace Minecraft.Commands;

public class CommandsHandler
{
    readonly Dictionary<string, ICommand> _commands = new();

    internal CommandsHandler() { }

    public bool RegisterCommand<T>(string label) where T : ICommand, new()
    {
        return RegisterCommand(label, new T());
    }

    public bool RegisterCommand(string label, ICommand handler)
    {
        return _commands.TryAdd(label, handler);
    }

    public bool UnregisterCommand(string label)
    {
        return _commands.Remove(label);
    }

    public void TryParse(string input, Player sender)
    {
        if (input == null || input.Length == 0 || input[0] != '/')
        {
            return;
        }

        string label = input.Substring(1, input.Contains(' ') ? input.IndexOf(' ') - 1 : input.Length - 1);

        if (_commands.TryGetValue(label, out ICommand? value))
        {
            value.OnCommand(sender, label, input, input.Split(' ').Skip(1).ToArray());
            return;
        }

        switch (label)
        {
            case "chunk":
                if (sender is ConsolePlayer)
                {
                    sender.SendMessage("I cannot send chunk to console!");
                    break;
                }
                sender.Connection?.SendPacket(new ChunkDataPacket(0, 0));
                break;
            case "blockpls":
                if (sender is ConsolePlayer)
                {
                    sender.SendMessage("I cannot add block to console's world!");
                    break;
                }
                sender.Connection?.SendPacket(new BlockChangePacket(0, 128, 0, 1, 0));
                break;
            case "mstest":
                MStream stream = new();
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
            case "dyntest":
                dynamic sheesh = new
                {
                    text = "Dab",
                    bold = true,
                    extra = new int[5]
                };

                byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(sheesh);
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
            case "colortest":
                sender.SendMessage(ChatColor.Green + "Green! " + ChatColor.Reset + ChatColor.Strikethrough + "Ohhh... " + ChatColor.Reset + ChatColor.Bold + " I am bold! " + ChatColor.Reset + "I am normal!");
                sender.SendMessage(ChatColor.Red + "Red pill " + ChatColor.Reset + "or " + ChatColor.Blue + "Blue " + ChatColor.Reset + "one?");
                break;
            case "ismyground":
                sender.SendMessage((sender.IsOnGround ? ChatColor.Green + "Yeah, True" : ChatColor.Red + "Uhhh, False") + ChatColor.Reset);
                break;
            default:
                sender.SendMessage("Unknown command. Type \"/help\" for help.");
                break;
        }
    }
}
