using Minecraft.Tools;

namespace Minecraft
{
    public class ChatColor : Enumeration
    {
        public const char CHARACTER = '\xA7';

        public static readonly ChatColor BLACK         = new ChatColor(0,  '0');
        public static readonly ChatColor DARK_BLUE     = new ChatColor(1,  '1');
        public static readonly ChatColor DARK_GREEN    = new ChatColor(2,  '2');
        public static readonly ChatColor DARK_AQUA     = new ChatColor(3,  '3');
        public static readonly ChatColor DARK_RED      = new ChatColor(4,  '4');
        public static readonly ChatColor DARK_PURPLE   = new ChatColor(5,  '5');
        public static readonly ChatColor GOLD          = new ChatColor(6,  '6');
        public static readonly ChatColor GRAY          = new ChatColor(7,  '7');
        public static readonly ChatColor DARK_GRAY     = new ChatColor(8,  '8');
        public static readonly ChatColor BLUE          = new ChatColor(9,  '9');
        public static readonly ChatColor GREEN         = new ChatColor(10, 'a');
        public static readonly ChatColor AQUA          = new ChatColor(11, 'b');
        public static readonly ChatColor RED           = new ChatColor(12, 'c');
        public static readonly ChatColor LIGHT_PURPLE  = new ChatColor(13, 'd');
        public static readonly ChatColor YELLOW        = new ChatColor(14, 'e');
        public static readonly ChatColor WHITE         = new ChatColor(15, 'f');

        public static readonly ChatColor OBFUSCATED    = new ChatColor(16, 'k');
        public static readonly ChatColor BOLD          = new ChatColor(17, 'l');
        public static readonly ChatColor STRIKETHROUGH = new ChatColor(18, 'm');
        public static readonly ChatColor UNDERLINE     = new ChatColor(19, 'n');
        public static readonly ChatColor ITALIC        = new ChatColor(20, 'o');
        public static readonly ChatColor RESET         = new ChatColor(21, 'r');

        public ChatColor(int id, char value) : base(id, CHARACTER.ToString() + value) { }
    }
}
