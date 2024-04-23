using System.Collections.Generic;
using System.Linq;

using Minecraft.Entities;
using Minecraft.Packets;

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
                    sender.SendMessage("Get real");
                    break;
                }
                sender.Connection?.SendPacket(new ChunkDataPacket(0, 0));
                break;
            case "blockpls":
                if (sender is ConsolePlayer)
                {
                    sender.SendMessage("Get real");
                    break;
                }
                sender.Connection?.SendPacket(new BlockChangePacket(0, 128, 0, 1, 0));
                break;
            case "overflow":
                byte[] mybytes = new byte[5];
                byte[] badbytes = new byte[8] { 0, 5, 1, 73, 23, 72, 64, 23 };
                badbytes.CopyTo(mybytes, 0);
                break;
            case "colortest":
                sender.SendMessage(ChatColor.Green + "Green! " + ChatColor.Reset + ChatColor.Strikethrough + "Ohhh..." + ChatColor.Reset + ChatColor.Bold + " I am bold! " + ChatColor.Reset + "I am normal!");
                sender.SendMessage(ChatColor.Red + "Red pill " + ChatColor.Reset + "or a " + ChatColor.Blue + "Blue " + ChatColor.Reset + "one?");
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
