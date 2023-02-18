using Minecraft.Entities;
using Minecraft.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Packets.Receivers;

public class PlayerLookPacketReceiver : IPacketReceiver
{
    public bool AllowOverride => false;

    public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
    {
        Player player = handler.Player;

        MStream stream = new MStream(rawPacket);

        stream.ReadByte();
        float yaw = stream.ReadFloat();
        float pitch = stream.ReadFloat();
        bool onGround = stream.ReadBoolean();

        player.IsOnGround = onGround;
        player.Location = new Location
        {
            X = player.Location.X,
            Y = player.Location.Y,
            Z = player.Location.Z,
            Pitch = pitch,
            Yaw = yaw,
            World = player.Location.World
        };

        return rawPacket.Skip(10);
    }
}
