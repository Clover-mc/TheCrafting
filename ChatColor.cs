using Minecraft.Tools;

namespace Minecraft
{
    public class ChatColor : Enumeration
    {
        public const char Character = '\xA7';

        public static ChatColor Black       { get; } = new(0,  '0');
        public static ChatColor DarkBlue    { get; } = new(1,  '1');
        public static ChatColor DarkGreen   { get; } = new(2,  '2');
        public static ChatColor DarkAqua    { get; } = new(3,  '3');
        public static ChatColor DarkRed     { get; } = new(4,  '4');
        public static ChatColor DarkPurple  { get; } = new(5,  '5');
        public static ChatColor Gold        { get; } = new(6,  '6');
        public static ChatColor Gray        { get; } = new(7,  '7');
        public static ChatColor DarkGray    { get; } = new(8,  '8');
        public static ChatColor Blue        { get; } = new(9,  '9');
        public static ChatColor Green       { get; } = new(10, 'a');
        public static ChatColor Aqua        { get; } = new(11, 'b');
        public static ChatColor Red         { get; } = new(12, 'c');
        public static ChatColor LightPurple { get; } = new(13, 'd');
        public static ChatColor Yellow      { get; } = new(14, 'e');
        public static ChatColor White       { get; } = new(15, 'f');

        public static ChatColor Obfuscated    { get; } = new(16, 'k');
        public static ChatColor Bold          { get; } = new(17, 'l');
        public static ChatColor Strikethrough { get; } = new(18, 'm');
        public static ChatColor Underline     { get; } = new(19, 'n');
        public static ChatColor Italic        { get; } = new(20, 'o');
        public static ChatColor Reset         { get; } = new(21, 'r');

        public ChatColor(int id, char value)
            : base(id, Character.ToString() + value) { }

        public static string operator +(ChatColor left, ChatColor right)
        {
            return $"{left}{right}";
        }
    }
}
