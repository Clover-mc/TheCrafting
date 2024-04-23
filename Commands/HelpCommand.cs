using System.Collections.Generic;
using System.Text;

using Minecraft.Entities;

namespace Minecraft.Commands;

public class HelpCommand : ICommand
{
    public bool OnCommand(IEntity sender, string label, string raw, string[] args)
    {
        if (sender is not Player player)
        {
            return false;
        }

        var sb = new StringBuilder()
            .AppendLine("== Help")
            .AppendLine(" | stop - shutdown server")
            .AppendLine(" | help - show this text")
            .AppendLine(" | teleport - teleport to coordinates")
            .Append(" | displayname - change display name");

        player.SendMessage(sb.ToString());

        return true;
    }

    public IList<string>? OnTabComplete(IEntity sender, string label, string raw, string[] args)
    {
        return null;
    }
}
