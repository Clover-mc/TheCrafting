namespace Minecraft;

public enum PermissionLevel
{
    /// <summary>
    /// Default permission level for everyone, nothing special
    /// </summary>
    Default,
    /// <summary>
    /// Player can bypass spawn protection
    /// </summary>
    BypassSpawn,
    /// <summary>
    /// Player can execute commands like /clear, /gamemode, /give, /seed, /tp, /weather, etc
    /// </summary>
    Cheats,
    /// <summary>
    /// Player can execute commands like /ban, /ban-ip, /pardon, /pardon-ip, /op, /deop, /kick, etc. +all from <see cref="Cheats"/>
    /// </summary>
    Moderator,
    /// <summary>
    /// Maximum permission level. Everything is allowed
    /// </summary>
    Op
}
