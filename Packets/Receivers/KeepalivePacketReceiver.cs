using System;
using System.Collections.Generic;
using System.Linq;
using Minecraft.Entities;

namespace Minecraft.Packets.Receivers
{
    public sealed class KeepalivePacketReceiver : IPacketReceiver
    {
        public bool AllowOverride => false;

        public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
        {
            Player player = handler.Player;
            KeepalivePacket packet = new KeepalivePacket(rawPacket);
            var message = player.KeepalivePending.Find(message => message.Packet.Payload == packet.Payload);

            DateTime time = DateTime.MinValue;
            if (message is not null)
            {
                time = message.Time;
            }
            else if (packet.Payload == 0)
            {
                var latestMessage = player.KeepalivePending.MaxBy(msg => msg.Time);
                if (latestMessage is not null)
                {
                    time = latestMessage.Time;
                }
            }

            if (time != DateTime.MinValue)
            {
                handler.Player.Ping = (short)(DateTime.Now - time).TotalMilliseconds;
                handler.Player.KeepalivePending.Clear();
            }

            return rawPacket.Skip(packet.PacketLength);
        }
    }
}
