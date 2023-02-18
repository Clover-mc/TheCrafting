using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Packets
{
    public class PlayerLookPacket : IPacket
    {
        private MStream Stream;

        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public bool OnGround { get; private set; }

        public PacketList Id => PacketList.PLAYER_LOOK;
        public int PacketLength => 10;
        public IEnumerable<byte> Raw => Stream.Array;

        public PlayerLookPacket(float yaw, float pitch, bool on_ground)
        {
            (Yaw, Pitch, OnGround) = (yaw, pitch, on_ground);

            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(Yaw);
            Stream.Write(Pitch);
            Stream.Write(OnGround);
        }
    }
}
