using System.Collections.Generic;
using System.Text;

using Minecraft.Entities;

namespace Minecraft.Commands
{
    public class HelpCommand : ICommand
    {
        public bool OnCommand(IEntity sender, string label, string raw, string[] args)
        {
            if (sender is null || sender is not Player) return false;

            Player player = sender as Player;

            StringBuilder sb = new StringBuilder("== Help\n");
            sb.AppendLine(" | exit - shutdown server");
            sb.AppendLine(" | help - show this text");
            sb.Append(" | clear - clear console");

            player.SendMessage(sb.ToString());

            return true;
        }

        public IList<string> OnTabComplete(IEntity sender, string label, string raw, string[] args)
        {
            return null;
        }
    }
}
