namespace Minecraft
{
    public enum Gamemode : byte
    {
        SURVIVAL  = 0,
        CREATIVE  = 1,
        ADVENTURE = 2,

        SURVIVAL_HARDCORE  = 0b1000,
        CREATIVE_HARDCORE  = 0b1001,
        ADVENTURE_HARDCORE = 0b1010
    }
}
