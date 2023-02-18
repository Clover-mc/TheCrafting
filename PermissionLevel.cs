using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft
{
    public enum PermissionLevel
    {
        /// <summary>
        /// Default permission level for everyone, nothing special
        /// </summary>
        DEFAULT,
        /// <summary>
        /// Player can bypass spawn protection
        /// </summary>
        BYPASS_SPAWN,
        /// <summary>
        /// Player can execute commands like /clear, /gamemode, /give, /seed, /tp, /weather, etc
        /// </summary>
        CHEATS,
        /// <summary>
        /// Player can execute commands like /ban, /ban-ip, /pardon, /pardon-ip, /op, /deop, /kick, etc. +all from CHEATS
        /// </summary>
        MODERATOR,
        /// <summary>
        /// Player can execute commands like /save-all, /save-on, /save-off, /stop, etc. +all from MODERATOR
        /// </summary>
        OP
    }
}
