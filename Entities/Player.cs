using System;
using System.Text.RegularExpressions;

using Minecraft.Packets;

namespace Minecraft.Entities
{
    public class Player : Entity
    {
        private readonly PlayerConnection Connection;
        private Guid UUID;
        private string Nickname;
        private string DisplayName;

        internal Player(PlayerConnection connection, string nickname)
        {
            Connection = connection;
            Nickname = nickname;
            DisplayName = nickname;
            UUID = Guid.Empty;
        }

        public void SendPacket(Packet packet)
        {
            Connection.SendPacket(packet);
        }

        /*public void SendMessage(string message)
        {
            Connection.SendPacket(new ChatMessagePacket(message));
        }*/

        public string GetName()
        {
            return Nickname;
        }

        public string GetDisplayName()
        {
            return DisplayName;
        }

        public Guid GetUUID()
        {
            return UUID;
        }

        public void SetDisplayName(string displayName)
        {
            DisplayName = displayName is null ? "" : displayName;
        }

        public PlayerConnection GetConnection()
        {
            return Connection;
        }

        internal byte[] ReceivePacket()
        {
            return Connection.Receive();
        }

        internal void SetName(string? nickname)
        {
            Nickname = nickname is null ? "" : nickname;
            if (GetDisplayName() == "") SetDisplayName(GetName());
        }

        internal void SetUUID(Guid uuid)
        {
            UUID = uuid;
        }

        public static bool IsValidNickname(string nickname)
        {
            if (nickname is null || nickname.Length < 3 || nickname.Length > 16 || Regex.IsMatch(nickname, @"[^\w_]")) return false;
            return true;
        }
    }
}
