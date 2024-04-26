using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Minecraft.Entities;
using Minecraft.Network.Packets;

namespace Minecraft.Network
{
    public class TabListHandler
    {
        public List<TabListPlayer> Players { get; } = new();

        bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (!Enabled && value != Enabled)
                {
                    Start();
                }
                _enabled = value;
            }
        }

        readonly MinecraftServer _server;

        internal TabListHandler(MinecraftServer server)
        {
            _server = server;
        }

        private void Start()
        {
            Task.Run(async () =>
            {
                while (Enabled)
                {
                    Update();

                    await Task.Delay(1000);
                }
            });
        }

        /// <summary>
        /// Updates info about players and sends it to everyone
        /// </summary>
        public void Update()
        {
            List<TabListPlayer> toRemove = Players.FindAll(player =>
                _server.Players.Any(serverPlayer => serverPlayer.Nickname.Equals(player.Nickname, StringComparison.OrdinalIgnoreCase)));

            foreach (Player player in _server.Players)
                foreach (TabListPlayer remove in toRemove)
                    if (player.Connection?.Connected == true)
                        player.Connection.SendPacketAsync(new PlayerListItemPacket(remove.DisplayName, false, 0));

            // Removing all real players from list ...
            Players.RemoveAll(player => !player.Dummy);

            // and adding them again!
            foreach (Player player in _server.Players)
                Players.Add(new TabListPlayer(player.Nickname)
                {
                    DisplayName = player.DisplayName,
                    IsOnline = player.Connection?.Connected == true,
                    Ping = player.Ping,
                    Dummy = false
                });

            foreach (Player player in _server.Players)
                foreach (TabListPlayer tabPlayer in Players)
                    if (player.Connection?.Connected == true)
                        player.Connection.SendPacketAsync(new PlayerListItemPacket(tabPlayer.DisplayName, tabPlayer.IsOnline, tabPlayer.Ping));
        }

        public struct TabListPlayer
        {
            public readonly string Nickname;

            public string DisplayName;
            public short Ping;
            public bool IsOnline;
            internal bool Dummy = true;

            public TabListPlayer()
                : this(string.Empty) { }

            public TabListPlayer(string nickname)
                => (Nickname, DisplayName, Ping, IsOnline, Dummy) = (nickname, "", 0, true, true);
        }
    }
}
