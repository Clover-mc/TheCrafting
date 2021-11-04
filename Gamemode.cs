namespace Minecraft
{
    public enum Gamemode : byte
    {
        SURVIVAL  = 0x00,
        CREATIVE  = 0x01,
        ADVENTURE = 0x02,

        SURVIVAL_HARDCORE  = 0x80,
        CREATIVE_HARDCORE  = 0x81,
        ADVENTURE_HARDCORE = 0x82
    }
}
