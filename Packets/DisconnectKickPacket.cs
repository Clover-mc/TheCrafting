namespace Minecraft.Packets
{
    public class DisconnectKickPacket : Packet
    {
        private MStream Stream;

        public DisconnectKickPacket(string reason)
        {
            Stream = new MStream();
            Stream.WriteByte(0xFF);
            Stream.Write(reason);
        }

        public override byte GetId()
        {
            return 0xFF;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
