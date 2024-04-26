using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Network.Packets
{
    public class PlayerPositionAndLookPacket : IPacket
    {
        private MStream Stream;

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public double Stance { get; private set; }
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public bool OnGround { get; private set; }

        public PacketList Id => PacketList.PlayerPositionAndLook;
        public int PacketLength => 42;
        public IEnumerable<byte> Raw => Stream.Array;

        public PlayerPositionAndLookPacket(bool fromServer, bool onGround, double stance, Location loc)
            : this(fromServer, loc.X, loc.Y, loc.Z, stance, loc.Yaw, loc.Pitch, onGround) { }

        public PlayerPositionAndLookPacket(bool fromServer, double x, double y, double z, double stance, float yaw, float pitch, bool onGround)
        {
            (X, Y, Z, Stance, OnGround) = (x, y, z, stance, onGround);

            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(X);
            if (fromServer)
            {
                Stream.Write(Stance);
                Stream.Write(Y);
            }
            else
            {
                Stream.Write(Y);
                Stream.Write(Stance);
            }
            Stream.Write(Z);
            Stream.Write(Yaw);
            Stream.Write(Pitch);
            Stream.Write(OnGround);
        }
    }
}
