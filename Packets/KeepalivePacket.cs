using System;
using System.Collections.Generic;
using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class KeepalivePacket : IPacket
    {
        private MStream Stream;

        public PacketList Id => PacketList.KEEPALIVE;
        public int PacketLength => 5;
        public IEnumerable<byte> Raw => Stream.Array;

        public int Payload { get; set; }

        public KeepalivePacket()
        {
            Stream = new MStream();
            Random random = new Random();
            Payload = random.Next(int.MinValue, int.MaxValue);

            Stream.WriteByte((byte)Id);
            Stream.Write(Payload);
        }

        public KeepalivePacket(int payload)
        {
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(payload);
        }

        public KeepalivePacket(IEnumerable<byte> packet)
        {
            Stream = new MStream(packet);
            if (Stream.Read() != (byte)Id) throw new ArgumentException("Given byte array is not " + nameof(HandshakePacket) + '!');

            Payload = Stream.ReadInt();
        }

        public class KeepaliveMesssage
        {
            public KeepalivePacket Packet { get; set; } = null!;
            public DateTime Time { get; set; }
        }
    }
}
