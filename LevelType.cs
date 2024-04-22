using Minecraft.Tools;

namespace Minecraft;

public class LevelType : Enumeration
{
    public static LevelType Default     { get; } = new(0, "default");
    
    public static LevelType Flat        { get; } = new(1, "flat");
    
    public static LevelType LargeBiomes { get; } = new(2, "largeBiomes");

    public LevelType(int id, string type)
        : base(id, type) { }
}
