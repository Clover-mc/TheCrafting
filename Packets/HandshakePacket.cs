using System;
using System.Collections.Generic;
using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class HandshakePacket : IPacket
    {
        private readonly MStream Stream;
        public byte ProtocolVersion { get; set; }
        public string Nickname { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }

        public int PacketLength { get; private set; }
        public PacketList Id => PacketList.HANDSHAKE;
        public IEnumerable<byte> Raw => Stream.Array;

        public HandshakePacket(byte protocolVersion, string nickname, string address, int port)
        {
            ProtocolVersion = protocolVersion;
            Nickname = nickname;
            ServerAddress = address;
            ServerPort = port;

            PacketLength = 10 + (Nickname.Length + ServerAddress.Length) * 2; // Every string is double-sized

            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(ProtocolVersion);
            Stream.Write(Nickname);
            Stream.Write(ServerAddress);
            Stream.Write(ServerPort);
        }

        public HandshakePacket(IEnumerable<byte> packet)
        {
            Stream = new MStream(packet);
            if (Stream.Read() != (byte)Id) throw new ArgumentException("Given byte array is not " + nameof(HandshakePacket) + '!');

            ProtocolVersion = Stream.ReadByte();
            Nickname = Stream.ReadString();
            ServerAddress = Stream.ReadString();
            ServerPort = Stream.ReadInt();

            PacketLength = 10 + (Nickname.Length + ServerAddress.Length) * 2; // Every string is double-sized
        }
    }
}
