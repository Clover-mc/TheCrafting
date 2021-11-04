namespace Minecraft.Packets
{
    public class HandshakePacket : IncomingPacket
    {
        private MStream Stream;
        private int ProtocolVersion;
        private string Nickname;
        private string ServerAddress;
        private int ServerPort;

        public HandshakePacket(int protocol_version, string nickname, string server_address, int server_port)
        {
            ProtocolVersion = protocol_version;
            Nickname = nickname;
            ServerAddress = server_address;
            ServerPort = server_port;

            Stream = new MStream();
            Stream.WriteByte(0x02);
            Stream.Write((short)ProtocolVersion);
            Stream.Write(Nickname);
            Stream.Write(ServerAddress);
            Stream.Write(ServerPort);
        }

        //public HandshakePacket(byte[] packet)
        //{
        //    Stream = MStream.From(packet);
        //    byte[] data = Stream.GetArray();
        //    if (data[0] != GetId()) throw new ArgumentException("Given byte array is not \"Handshake Packet\"!");
        //}

        public override byte GetId()
        {
            return 0x02;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
