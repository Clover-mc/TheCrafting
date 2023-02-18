using Minecraft.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minecraft.Packets;

public class DestroyEntityPacket : IPacket
{
    private MStream Stream;

    public int PacketLength { get; private set; }
    public PacketList Id => PacketList.DESTROY_ENTITY;
    public IEnumerable<byte> Raw => Stream.Array;

    public DestroyEntityPacket(IEnumerable<uint> entityIds)
    {
        if (entityIds.Count() > 0xFF)
            throw new ArgumentException("Array is too big to fit into one packet! (Count > 0xFF)", nameof(entityIds));

        Stream = new();

        Stream.Write((byte)Id);
        Stream.WriteByte((byte)entityIds.Count());

        foreach (uint id in entityIds)
            Stream.Write((int)id);

        PacketLength = (int)Stream.Position;
    }

    public DestroyEntityPacket(uint id)
    {
        Stream = new();

        Stream.Write((byte)Id);
        Stream.WriteByte(1);
        Stream.Write((int)id);

        PacketLength = 6;
    }
}
