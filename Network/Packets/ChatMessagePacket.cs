using Minecraft.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft.Network.Packets;

public sealed class ChatMessagePacket : IPacket
{
    public string Text { get; set; }

    public PacketList Id => PacketList.ChatMessage;

    public int PacketLength => 3 + Text.Length * 2;
    
    public IEnumerable<byte> Raw => new[] { (byte)Id }
        .Concat(BitConverter.GetBytes((short)Text.Length).BigEndian())
        .Concat(Encoding.BigEndianUnicode.GetBytes(Text));

    public ChatMessagePacket(string text)
    {
        Text = text;
    }

    public ChatMessagePacket(IEnumerable<byte> packet)
    {
        var stream = new MStream(packet);
        if (stream.Read() != (byte)Id)
        {
            stream.Close();
            throw new ArgumentException($"Given byte array is not {nameof(ChatMessagePacket)}!");
        }

        Text = stream.ReadString();

        stream.Close();
    }
}
