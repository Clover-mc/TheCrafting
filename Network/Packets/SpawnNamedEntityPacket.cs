using Minecraft.Tools;
using System.Collections.Generic;

namespace Minecraft.Network.Packets;

public class SpawnNamedEntityPacket : IPacket
{
    private MStream Stream;

    public int PacketLength { get; private set; }
    public PacketList Id => PacketList.SpawnNamedEntity;
    public IEnumerable<byte> Raw => Stream.Array;

    public uint EntityId { get; private set; }
    public string Name { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }
    public byte Yaw { get; private set; }
    public byte Pitch { get; private set; }
    public short CurrentItem { get; private set; }


    public SpawnNamedEntityPacket(uint entityId, string name, Location location, short currentItem)
    {
        Stream = new MStream();

        if (name.Length > 16)
            name = name[..16];

        Stream.Write((byte)Id);
        Stream.Write((int)(EntityId = entityId));
        Stream.Write(Name = name);
        Stream.Write(location.X);
        Stream.Write(location.Y);
        Stream.Write(location.Z);
        Stream.WriteByte(0);
        Stream.WriteByte(0);
        Stream.Write(CurrentItem = currentItem);

        Stream.Write((short)0);
        Stream.WriteByte(0b01100110);
        Stream.Write(20f);
        Stream.WriteByte(0x7F);
    }
}
