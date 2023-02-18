using System.Collections.Generic;

namespace Minecraft.Packets;

/// <summary>
/// Base interface for every network packet
/// </summary>
public interface IPacket
{
    /// <summary>
    /// Total length of raw packet
    /// </summary>
    int PacketLength { get; }

    /// <summary>
    /// Id of packet
    /// </summary>
    PacketList Id { get; }

    // TODO: Generate and cache MStream as soon as Property accessed, not when packet created
    /// <summary>
    /// Raw packet as byte sequence
    /// </summary>
    IEnumerable<byte> Raw { get; }
}
