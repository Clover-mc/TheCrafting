namespace Minecraft;

public enum GameMode : byte
{
    Survival  = 0,
    Creative  = 1,
    Adventure = 2,

    HardcoreFlag  = 1 << 3
}
