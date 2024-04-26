using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Network.Packets
{
    public class PlayerPositionPacket : IPacket
    {
        private MStream Stream;

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public double Stance { get; private set; }
        public bool OnGround { get; private set; }

        public PacketList Id => PacketList.PlayerPosition;
        public int PacketLength => 34;
        public IEnumerable<byte> Raw => Stream.Array;

        public PlayerPositionPacket(double x, double y, double z, double stance, bool on_ground)
        {
            (X, Y, Z, Stance, OnGround) = (x, y, z, stance, on_ground);

            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(X);
            Stream.Write(Y);
            Stream.Write(Stance);
            Stream.Write(Z);
            Stream.Write(OnGround);
        }
    }
}
