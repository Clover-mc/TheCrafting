using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Network.Packets.Receivers;

public sealed class ChatMessagePacketReceiver : IPacketReceiver
{
    public bool AllowOverride => true;

    public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
    {
        if (rawPacket.Count() < 3 || rawPacket.ElementAt(0) != (byte)PacketList.ChatMessage)
        {
            return rawPacket;
        }

        var player = handler.Player;
        var packet = new ChatMessagePacket(rawPacket);

        var text = packet.Text.Trim();

        if (text.StartsWith('/'))
        {
            server.Commands.TryParse(text, player);
        }
        else
        {
            string message = $"<{player.DisplayName}> {text}";

            Log.Information(message);
            server.BroadcastMessage(message);
        }

        return rawPacket.Skip(packet.PacketLength);
    }
}
