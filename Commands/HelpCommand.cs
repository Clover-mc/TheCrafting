using System.Collections.Generic;
using System.Text;

using Minecraft.Entities;

namespace Minecraft.Commands
{
    public class HelpCommand : ICommand
    {
        public bool OnCommand(IEntity sender, string label, string raw, string[] args)
        {
            if (sender is null || sender is not Player player)
            {
                return false;
            }

            var sb = new StringBuilder()
                .AppendLine("== Help")
                .AppendLine(" | exit - shutdown server")
                .AppendLine(" | help - show this text")
                .Append(" | clear - clear console");

            player.SendMessage(sb.ToString());

            return true;
        }

        public IList<string>? OnTabComplete(IEntity sender, string label, string raw, string[] args)
        {
            return null;
        }
    }
}
