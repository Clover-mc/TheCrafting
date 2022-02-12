namespace Minecraft.Chat.Builder
{
    public class TextComponent
    {
#pragma warning disable IDE1006 // Naming Styles
        public string text { get; set; }
        public string bold { get; set; }
        public string italic { get; set; }
        public string underlined { get; set; }
        public string strikethrough { get; set; }
        public string obfuscated { get; set; }
        public string font { get; set; }
        public string color { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        public TextComponent(string text, bool bold, bool italic, bool underlined,
            bool strikethrough, bool obfuscated, string font, string color)
        {
            this.text   = text;
            this.bold   = Convert(bold);
            this.font   = font;
            this.color  = color;
            this.italic = Convert(italic);
            this.underlined    = Convert(underlined);
            this.obfuscated    = Convert(obfuscated);
            this.strikethrough = Convert(strikethrough);
        }

        private static string Convert(bool b)
        {
            return b.ToString().ToLower();
        }
    }
}
