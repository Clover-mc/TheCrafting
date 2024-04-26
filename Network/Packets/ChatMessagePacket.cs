using Minecraft.Tools;
using System;
using System.Collections.Generic;

namespace Minecraft.Network.Packets
{
    public class ChatMessagePacket : IPacket
    {
        private MStream Stream;

        public string Text { get; set; }

        public PacketList Id => PacketList.ChatMessage;
        public int PacketLength { get; private set; }
        public IEnumerable<byte> Raw => Stream.Array;

        public ChatMessagePacket(string text)
        {
            Stream = new MStream();
            Stream.WriteByte((byte)Id);
            Stream.Write(text);
            Text = text;
            PacketLength = 3 + text.Length * 2; // Every string is double-sized
        }

        public ChatMessagePacket(IEnumerable<byte> packet)
        {
            Stream = new MStream(packet);
            if (Stream.Read() != (byte)Id) throw new ArgumentException("Given byte array is not " + nameof(ChatMessagePacket) + '!');

            Text = Stream.ReadString();

            PacketLength = 10 + Text.Length * 2; // Every string is double-sized
        }
    }
}
