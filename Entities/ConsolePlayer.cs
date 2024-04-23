using Serilog;
using System.Text.RegularExpressions;

namespace Minecraft.Entities;

public class ConsolePlayer : Player
{
    readonly Regex _colorRegex = new(ChatColor.Character + ".", RegexOptions.Compiled);

    internal ConsolePlayer()
        : base()
    {
        Nickname = string.Empty;
    }

    public override void SendMessage(string message)
    {
        Log.Information(_colorRegex.Replace(message, string.Empty));
    }
}
