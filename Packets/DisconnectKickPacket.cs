using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class DisconnectKickPacket : IPacket
    {
        public string Reason { get; set; }

        public PacketList Id => PacketList.DISCONNECT_KICK;

        public int PacketLength => 3 + Reason.Length * 2;
        
        public IEnumerable<byte> Raw => new[] { (byte)Id }
            .Concat(BitConverter.GetBytes((short)Reason.Length).ToBigEndian())
            .Concat(Encoding.BigEndianUnicode.GetBytes(Reason));

        public DisconnectKickPacket(string reason)
        {
            Reason = reason;
        }

        public DisconnectKickPacket(byte[] packet)
        {
            var stream = new MStream(packet);
            if (stream.Read() != (byte)Id)
            {
                stream.Close();
                throw new ArgumentException($"Given byte array is not {nameof(DisconnectKickPacket)}!");
            }

            Reason = stream.ReadString();

            stream.Close();
        }
    }
}
