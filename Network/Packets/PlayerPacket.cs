using System;
using System.Collections.Generic;
using Minecraft.Tools;

namespace Minecraft.Network.Packets
{
    public class PlayerPacket : IPacket
    {
        private readonly MStream Stream;

        public bool IsOnGround { get; set; }

        public int PacketLength => 2;
        public PacketList Id => PacketList.Player;
        public IEnumerable<byte> Raw => Stream.Array;

        public PlayerPacket(bool isOnGround)
        {
            Stream = new MStream();

            IsOnGround = isOnGround;

            Stream.Write((byte)Id);
            Stream.Write(IsOnGround);
        }

        public PlayerPacket(IEnumerable<byte> rawPacket)
        {
            Stream = new MStream(rawPacket);

            if (Stream.Read() != (byte)Id) throw new ArgumentException("Given byte array is not \"" + nameof(PlayerPacket) + "\"!");

            IsOnGround = Stream.ReadBoolean();
        }
    }
}
