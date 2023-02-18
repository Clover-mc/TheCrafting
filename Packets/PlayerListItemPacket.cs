using System;
using System.Collections.Generic;
using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class PlayerListItemPacket : IPacket
    {
        private readonly MStream Stream;

        public string Player { get; set; }
        public bool Online { get; set; }
        public short Ping { get; set; }

        public int PacketLength { get; private set; }
        public PacketList Id => PacketList.PLAYER_LIST_ITEM;
        public IEnumerable<byte> Raw => Stream.Array;

        public PlayerListItemPacket(string player, bool online, short ping)
        {
            Stream = new MStream();

            Player = player;
            Online = online;
            Ping = ping;

            Stream.Write((byte)Id);
            Stream.Write(Player);
            Stream.Write(Online);
            Stream.Write(Ping);

            PacketLength = 6 + Player.Length * 2; // Every string is double-sized
        }

        public PlayerListItemPacket(byte[] rawPacket)
        {
            Stream = new MStream(rawPacket);

            if (Stream.Read() != (byte)Id) throw new ArgumentException("Given byte array is not \"" + nameof(PlayerListItemPacket) + "\"!");

            Player = Stream.ReadString();
            Online = Stream.ReadBoolean();
            Ping = Stream.ReadShort();

            PacketLength = 6 + Player.Length * 2; // Every string is double-sized
        }
    }
}
