namespace Minecraft.Chat.Builder
{
    public class ItalicText : TextComponent
    {
        public ItalicText(string text) : base(text, false, true, false, false, false, "minecraft:default", "#ffaa00")
        {
            this.text = text;
            italic = "true";
        }
    }
}
