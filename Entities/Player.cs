using System;
using System.Collections.Generic;

using Minecraft.Packets;

namespace Minecraft.Entities
{
    public class Player : IEntity
    {
        public uint EntityId { get; set; }
        public PlayerConnection Connection { get; private set; }
        public string Nickname {
            get => _nickname;
            internal set
            {
                _nickname = value ?? "";
                if (Connection is not null) Connection.Nickname = value ?? "";
            }
        }
        public string DisplayName { get => _displayName; set => _displayName = value ?? Nickname; }
        public bool IsOnGround { get; set; }
        public short Ping { get; set; }
        public Location Location
        {
            get => _location;
            set
            {
                if (value.World is null)
                    throw new ArgumentNullException(nameof(value), "World cannot be null!");
                _location = value;
            }
        }

        internal ulong KeepaliveIndex;
        /// <summary>
        /// Keepalive packets that player didn't answer
        /// </summary>
        internal List<KeepalivePacket.KeepaliveMesssage> KeepalivePending;

        private string _nickname;
        private string _displayName;
        private Location _location;


        internal Player() : base()
        {
            Connection = null;
            Nickname = "DummyPlayer";
            DisplayName = "Dummy Player";
            Location = new Location();

            KeepalivePending = new List<KeepalivePacket.KeepaliveMesssage>();
        }

        internal Player(PlayerConnection connection, string nickname)
        {
            Connection = connection;
            Nickname = nickname;
            DisplayName = nickname;

            KeepalivePending = new List<KeepalivePacket.KeepaliveMesssage>();
        }

        public virtual void SendMessage(string message)
        {
            Connection.SendPacket(new ChatMessagePacket(message));
        }

        public virtual void Disconnect(string message = "Disconnected.")
        {
            Connection.SendPacket(new DisconnectKickPacket(message));
            Connection.Close();

            foreach (Player player in Connection.Server.Players)
                player.Connection.SendPacketAsync(new DestroyEntityPacket(EntityId));
        }

        internal byte[] ReceivePacket()
        {
            return Connection.Receive();
        }

        /// <summary>
        /// Checks nickname for Mojang's nickname standard
        /// </summary>
        /// <param name="nickname">Nickname of player</param>
        /// <returns>
        /// 0 - OK
        /// 1 - Nickname is too short (length < 3)
        /// 2 - Nickname is too long (length > 16)
        /// 3 - Nickname contains invalid characters
        /// 4 - Nickname is Null
        /// </returns>
        public static int ValidateNickname(string nickname)
        {
            if (nickname is null) return 4;
            else if (nickname.Length < 3)  return 1;
            else if (nickname.Length > 16) return 2;
            else if (!ValidateNicknameCharacters(nickname)) return 3;

            return 0;
        }

        private static bool ValidateNicknameCharacters(string nickname)
        {
            nickname = nickname.ToLower();
            for (int i = 0; i < nickname.Length; i++)
                if (!((nickname[i] >= 'a' && nickname[i] <= 'z') ||
                    (nickname[i] >= '0' && nickname[i] <= '9')) &&
                    nickname[i] != '_')
                    return false;
            return true;
        }

    }
}
