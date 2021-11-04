namespace Minecraft.Packets
{
    public class ServerListPingPacket : OutgoingPacket
    {
        private MStream Stream;
        public ServerListPingPacket(char   ping_version,   int protocol_version,
                                    string version_name,   string description,
                                    int    online_players, int    max_players) : base()
        {
            Stream = new MStream();

            Stream.WriteByte(0xFF);
            Stream.Write('\xA7' + ping_version.ToString() +
                '\0' + protocol_version.ToString() +
                '\0' + version_name +
                '\0' + description +
                '\0' + online_players.ToString() +
                '\0' + max_players.ToString());
        }

        public override byte GetId()
        {
            return 0xFE;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
