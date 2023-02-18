using System;
using System.Collections.Generic;
using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class DisconnectKickPacket : IPacket
    {
        private MStream Stream;

        public string Reason { get; set; }

        public PacketList Id => PacketList.DISCONNECT_KICK;
        public int PacketLength { get; private set; }
        public IEnumerable<byte> Raw => Stream.Array;

        public DisconnectKickPacket(string reason)
        {
            Reason = reason;
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(reason);

            PacketLength = (int)Stream.Position;
        }

        public DisconnectKickPacket(byte[] packet)
        {
            Stream = new MStream(packet);
            if (Stream.Read() != (byte)Id) throw new ArgumentException("Given byte array is not \"Disconnect Kick Packet\"!");

            Reason = Stream.ReadString();
            PacketLength = 3 + Reason.Length * 2; // Every string is double-sized
        }
    }
}
