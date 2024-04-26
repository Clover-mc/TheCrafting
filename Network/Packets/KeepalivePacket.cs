using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Minecraft.Tools;

namespace Minecraft.Network.Packets
{
    public class KeepalivePacket : IPacket
    {
        readonly MStream _stream;

        public PacketList Id => PacketList.Keepalive;
        public int PacketLength => 5;
        public IEnumerable<byte> Raw => _stream.Array;

        public int Payload { get; set; }

        public KeepalivePacket()
        {
            _stream = new MStream();
            Payload = RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue);

            _stream.WriteByte((byte)Id);
            _stream.Write(Payload);
        }

        public KeepalivePacket(int payload)
        {
            _stream = new MStream();
            _stream.WriteByte((byte)Id);
            _stream.Write(payload);
        }

        public KeepalivePacket(IEnumerable<byte> packet)
        {
            _stream = new MStream(packet);
            if (_stream.Read() != (byte)Id) throw new ArgumentException("Given byte array is not " + nameof(HandshakePacket) + '!');

            Payload = _stream.ReadInt();
        }

        public struct KeepaliveMessage
        {
            public int Payload;
            public DateTime Time;

            public KeepaliveMessage(int payload, DateTime time)
            {
                Payload = payload;
                Time = time;
            }
        }
    }
}
