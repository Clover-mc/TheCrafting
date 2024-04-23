using Minecraft.Entities;
using System.Collections.Generic;

namespace Minecraft.Commands;

public class StopCommand : ICommand
{
    public bool OnCommand(IEntity sender, string label, string raw, string[] args)
    {
        if (sender is not Player player || player.Connection is null)
        {
            return false;
        }

        player.Connection.Server.Stop();

        return true;
    }

    public IList<string>? OnTabComplete(IEntity sender, string label, string raw, string[] args)
    {
        return null;
    }
}
