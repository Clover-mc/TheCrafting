using Minecraft.Entities;
using Minecraft.Packets;
using System.Collections.Generic;

namespace Minecraft.Commands;

public class TeleportCommand : ICommand
{
    public bool OnCommand(IEntity sender, string label, string raw, string[] args)
    {
        if (sender is not Player player || player.Connection is null || player is ConsolePlayer)
        {
            return false;
        }

        if (args.Length != 3)
        {
            player.SendMessage(ChatColor.Red + "Wrong number of arguments!" + ChatColor.Reset);
            return true;
        }

        if (!float.TryParse(args[0], out var x))
        {
            player.SendMessage(ChatColor.Red + "Unable to parse X coordinate!" + ChatColor.Reset);
            return true;
        }

        if (!float.TryParse(args[1], out var y))
        {
            player.SendMessage(ChatColor.Red + "Unable to parse Y coordinate!" + ChatColor.Reset);
            return true;
        }

        if (!float.TryParse(args[2], out var z))
        {
            player.SendMessage(ChatColor.Red + "Unable to parse Z coordinate!" + ChatColor.Reset);
            return true;
        }

        player.Connection.SendPacket(new PlayerPositionPacket(x, y, z, 0, false));
        return true;
    }

    public IList<string>? OnTabComplete(IEntity sender, string label, string raw, string[] args)
    {
        return null;
    }
}
