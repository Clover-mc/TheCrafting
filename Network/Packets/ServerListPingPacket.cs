using Minecraft.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft.Network.Packets;

public sealed class ServerListPingPacket : IPacket
{
    public byte Magic { get; set; }

    public PacketList Id => PacketList.ServerListPing;

    public int PacketLength => 2;
    
    public IEnumerable<byte> Raw => new[] { (byte)Id, Magic };

    public static DisconnectKickPacket GetResponse(
        byte pingVersion,
        int protocolVersion,
        string versionName,
        string description,
        int onlinePlayers,
        int maxPlayers)
    {
        return new DisconnectKickPacket(new[] { (byte)PacketList.DisconnectKick }
            .Concat(BitConverter.GetBytes(24 + versionName.Length * 2 + description.Length * 2))
            .Append((byte)0xA7)
            .Append(pingVersion)
            .Append((byte)0)
            .Concat(BitConverter.GetBytes(protocolVersion).BigEndian())
            .Append((byte)0)
            .Concat(BitConverter.GetBytes((short)versionName.Length).BigEndian())
            .Concat(Encoding.BigEndianUnicode.GetBytes(versionName))
            .Append((byte)0)
            .Concat(BitConverter.GetBytes((short)description.Length).BigEndian())
            .Concat(Encoding.BigEndianUnicode.GetBytes(description))
            .Append((byte)0)
            .Concat(BitConverter.GetBytes(onlinePlayers).BigEndian())
            .Append((byte)0)
            .Concat(BitConverter.GetBytes(maxPlayers).BigEndian()));
    }

    public static DisconnectKickPacket GetLegacyResponse(
        string description,
        int onlinePlayers,
        int maxPlayers)
    {
        return new DisconnectKickPacket($"{description}\xA7{onlinePlayers}\xA7{maxPlayers}");
    }
}
