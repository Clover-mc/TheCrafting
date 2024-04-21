using System;
using System.Collections.Generic;
using System.Linq;
using Minecraft.Entities;

namespace Minecraft.Packets.Receivers
{
    public sealed class HandshakePacketReceiver : IPacketReceiver
    {
        public bool AllowOverride => false;

        public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
        {
            Player player = handler.Player;
            HandshakePacket packet = new HandshakePacket(rawPacket);


            switch (Player.ValidateNickname(packet.Nickname))
            {
                case 1:
                    player.Disconnect("Your nickname is too short!");
                    break;
                case 2:
                    player.Disconnect("Your nickname is too long!");
                    break;
                case 3:
                    player.Disconnect("Your nickname contains invalid characters!");
                    break;
                case 4:
                    player.Disconnect("Your nickname is empty!");
                    break;
                case 0:
                default:
                    break;
            }

            if (handler.Connected)
            {
                IEnumerable<Player> simillar = server.Players.Where(playerList => playerList.Nickname.ToLower() == player.Nickname.ToLower());
                if (simillar.Any())
                {
                    player.Disconnect("You logged in from another location");
                    return Array.Empty<byte>();
                }

                player.DisplayName = player.Nickname = packet.Nickname;

                player.Connection.SendPacket(new LoginRequestPacket(0, LevelType.DEFAULT, Gamemode.CREATIVE, Dimension.OVERWORLD, Difficulty.NORMAL, (byte)(server.Config.MaxPlayers > 255 ? 255 : server.Config.MaxPlayers)));
                player.Connection.SendPacket(new PlayerPositionAndLookPacket(true, 0, 70, 0, 0, 0, 0, false));
                player.Connection.SendPacket(new MapChunkBulkPacket(49, true, new byte[] { 0 }));
                player.Connection.SendPacket(new PlayerListItemPacket(player.DisplayName, true, 10));

                handler.IsPlayer = true;

                Console.WriteLine("[WORLD] " + player.DisplayName + " joined the game");
                server.BroadcastMessage(ChatColor.Yellow + player.DisplayName + " joined the game");

                player.KeepaliveIndex = server.Interval.Create(id =>
                {
                    if (player.Connection.Connected)
                    {
                        if (player.KeepalivePending.Count >= 12)
                        {
                            player.Disconnect("Read timed out");
                            server.Interval.Cancel(id);
                            return;
                        }
                        KeepalivePacket packet = new KeepalivePacket();
                        player.KeepalivePending.Add(new KeepalivePacket.KeepaliveMesssage { Packet = packet, Time = DateTime.Now });
                        player.Connection.SendPacket(packet);
                    }
                    else
                    {
                        server.Interval.Cancel(id);
                        player.Disconnect();
                    }
                }, TimeSpan.FromSeconds(1), true);

                uint id = server.Worlds.ElementAt(0).RegisterEntity(player);

                StartTicking(handler, server);

                return rawPacket.Skip(packet.PacketLength);
            }
            else return Array.Empty<byte>();
        }

        private void StartTicking(ConnectionHandler handler, MinecraftServer server)
        {
            handler.TickId = server.Interval.Create(id =>
            {
                if (!handler.Connected)
                {
                    server.Interval.Cancel(id);
                    return;
                }

                foreach (Player player in server.Worlds[0].Entities.Where(e => e.Value is Player).Select(e => (Player)e.Value).Where(p => p.Connection.Connected && p.EntityId != handler.Player.EntityId))
                    player.Connection.SendPacketAsync(new PlayerPositionAndLookPacket(true, player.IsOnGround, player.Location.Y + 0.2, player.Location));
            }, TimeSpan.FromMicroseconds(1000 / 20));
        }

        private static byte[] GenerateChunks()
        {
            byte[] full = new byte[602112];

            for (int i = 0; i < full.Length; i++)
            {
                full[i] = 0x01;
            }

            return full;
        }
    }
}
