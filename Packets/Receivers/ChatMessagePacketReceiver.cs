using System;
using System.Collections.Generic;
using System.Linq;
using Minecraft.Entities;

namespace Minecraft.Packets.Receivers
{
    public sealed class ChatMessagePacketReceiver : IPacketReceiver
    {
        public bool AllowOverride => true;

        public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
        {
            Player player = handler.Player;
            ChatMessagePacket packet = new ChatMessagePacket(rawPacket);

            packet.Text = packet.Text.Trim();

            if (packet.Text.StartsWith('/')) server.Commands.TryParse(packet.Text, player);
            else
            {
                string message = '<' + handler.Player.DisplayName + "> " + packet.Text;

                Console.WriteLine(message);
                server.BroadcastMessage(message);
            }

            return rawPacket.Skip(packet.PacketLength);
        }
    }
}
