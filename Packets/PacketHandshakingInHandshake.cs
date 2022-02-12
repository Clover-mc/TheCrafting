using System;

using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class PacketHandshakingInHandshake : Packet
    {
        private readonly MStream Stream;
        private readonly string ServerAddress;
        private readonly VarInt ProtocolVersion;
        private readonly ushort ServerPort;
        private readonly VarInt NextState;

        public PacketHandshakingInHandshake(int protocol_version, string server_address, ushort server_port, int next_state)
        {
            ProtocolVersion = new VarInt(protocol_version);
            ServerAddress = server_address;
            ServerPort = server_port;
            NextState = new VarInt(next_state);

            Stream = new MStream();
            //(byte)IPacketIncoming.GetIncomingId()
            Stream.WriteByte(0);
            Stream.Write(VarInt.From(protocol_version).ToPackedArray());
            Stream.Write(0);
            Stream.Write(ServerPort);
            Stream.Write(NextState);

            byte[] bytes = Stream.GetArray();

            Stream = new MStream();
            Stream.Write(new VarInt(bytes.Length));
            Stream.Write(bytes);
        }

        public PacketHandshakingInHandshake(byte[] packet)
        {
            Stream = MStream.From(packet);

            Stream.ReadVarInt();
            VarInt id = Stream.ReadVarInt();

            if (id.ToInt() != GetId()) throw new ArgumentException("Given byte array is not \"Handshake Packet\"!");

            ProtocolVersion = Stream.ReadVarInt();
            ServerAddress = Stream.ReadString();

            ServerPort = (ushort)Stream.ReadShort();
            NextState = Stream.ReadVarInt();

            Console.WriteLine("== Results");
            Console.WriteLine("Protocol Version (754): " + ProtocolVersion.ToInt());
            Console.WriteLine("Server Address (127.0.0.1): " + ServerAddress);
            Console.WriteLine("Server Port (25565): " + ServerPort);
            Console.WriteLine("Next State (1): " + NextState.ToInt());

            Console.WriteLine("=============");
            //ProtocolVersion = Stream.ReadByte();
            //Nickname = Stream.ReadString();
            //ServerAddress = Stream.ReadString();
            //ServerPort = Stream.ReadInt();
        }

        public string GetServerAddress()
        {
            return ServerAddress;
        }
        public ushort GetServerPort()
        {
            return ServerPort;
        }

        public VarInt GetNextState()
        {
            return NextState;
        }

        public override int GetId()
        {
            return (int)Id.HandshakingInId.HANDSHAKE;
        }

        public override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
