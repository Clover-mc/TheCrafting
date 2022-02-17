using System;
//using System.Security.Cryptography;
using System.Text.RegularExpressions;

using Minecraft.Command;
using Minecraft.Chat.Builder;
using Minecraft.Entities;
using Minecraft.Packets;
using Minecraft.Packets.Id;
using Minecraft.Tools;

namespace Minecraft.Connection.StateHandlers
{
    internal class LoginStateHandler : StateHandler
    {
        private static bool IsCracked = true;
        public static LoginStateHandler Instance = new LoginStateHandler();
        internal override void Handle(Player player, byte[] packet)
        {
            MStream stream = MStream.From(packet);
            stream.ReadVarInt();
            LoginInId id = (LoginInId)stream.ReadVarInt().ToInt();

            // Login Start Packet
            if (id == LoginInId.LOGIN_START)
            {
                string nickname = stream.ReadString();

                if (CommandHandler.GetNickname() != "")
                {
                    nickname = CommandHandler.GetNickname();
                }

                if (nickname.Length < 3)
                {
                    PlayerLoginKick(new ItalicText("Nickname is too short"), player);
                    return;
                }
                if (nickname.Length > 16)
                {
                    PlayerLoginKick(new ItalicText("Nickname is too long"), player);
                    return;
                }
                if (Regex.IsMatch(nickname, @"[^\w_]"))
                {
                    PlayerLoginKick(new ItalicText("Nickname contains invalid characters"), player);
                    return;
                }

                player.SetName(nickname);
                Guid uuid = UUID.FromString("OfflinePlayer:" + player.GetName());
                player.SetUUID(uuid);

                Console.WriteLine("== Successful connect");
                Console.WriteLine("  Username: " + player.GetName());
                Console.WriteLine("  UUID: " + uuid.ToString());

                player.SendPacket(new PacketLoginOutLoginSuccess(uuid, player.GetName()));
                player.GetConnection().SetState(PlayerConnectionState.PLAY);

            }
            // Encryption Response Packet
            else if (id == LoginInId.ENCRYPTION_RESPONSE)
            {
                //
            }
            // Load Plugin Response Packet
            else if (id == LoginInId.LOGIN_PLUGIN_RESPONSE)
            {
                //
            }
        }

        public static void SetCracked(bool cracked)
        {
            IsCracked = cracked;
        }
        public static bool GetCracked()
        {
            return IsCracked;
        }

        private static void PlayerLoginKick(TextComponent text, Player player)
        {
            MStream kick = new MStream();
            kick.Write(new VarInt((int)LoginOutId.DISCONNECT_LOGIN));
            kick.Write(Json.JsonToString(text));
            player.GetConnection().SendRaw(EffectiveTools.PackPacket(kick));
            player.GetConnection().CloseConnection();
        }
    }
}
