using Minecraft.Entities;
using System.Collections.Generic;

namespace Minecraft.Commands;

public class DisplayNameCommand : ICommand
{
    public bool OnCommand(IEntity sender, string label, string raw, string[] args)
    {
        if (sender is not Player player || player is ConsolePlayer)
        {
            return false;
        }

        if (args.Length > 1)
        {
            player.SendMessage(ChatColor.Red + "Wrong number of arguments!" + ChatColor.Reset);
            return true;
        }

        player.DisplayName = args.Length == 0 ? null : args[0];
        return true;
    }

    public IList<string>? OnTabComplete(IEntity sender, string label, string raw, string[] args)
    {
        return null;
    }
}
