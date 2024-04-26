using System;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Network.Packets.Receivers;

public sealed class KeepalivePacketReceiver : IPacketReceiver
{
    public bool AllowOverride => false;

    public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
    {
        var player = handler.Player;
        var packet = new KeepalivePacket(rawPacket);
        var messageIndex = player.KeepalivePending.FindIndex(message => message.Payload == packet.Payload);

        DateTime? time = null;
        if (messageIndex != -1)
        {
            time = player.KeepalivePending[messageIndex].Time;
        }
        else if (packet.Payload == 0)
        {
            time ??= player.KeepalivePending.Max(msg => msg.Time as DateTime?);
        }

        if (time.HasValue)
        {
            handler.Player.Ping = (short)(DateTime.Now - time.Value).TotalMilliseconds;
            handler.Player.KeepalivePending.Clear();
        }

        return rawPacket.Skip(packet.PacketLength);
    }
}
