using Minecraft.Entities;
using Minecraft.Tools;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Packets.Receivers;

public class PlayerPositionAndLookPacketReceiver : IPacketReceiver
{
    public bool AllowOverride => false;

    public IEnumerable<byte> Process(ConnectionHandler handler, MinecraftServer server, IEnumerable<byte> rawPacket)
    {
        Player player = handler.Player;

        MStream stream = new MStream(rawPacket);

        stream.ReadByte();
        double x = stream.ReadDouble();
        double y = stream.ReadDouble();
        double stance = stream.ReadDouble();
        double z = stream.ReadDouble();
        float yaw = stream.ReadFloat();
        float pitch = stream.ReadFloat();
        bool onGround = stream.ReadBoolean();

        player.IsOnGround = onGround;
        player.Location = new Location
        {
            X = x,
            Y = y,
            Z = z,
            Pitch = pitch,
            Yaw = yaw,
            World = player.Location.World
        };

        return rawPacket.Skip(42);
    }
}
