namespace Minecraft.Packets
{
    public class ChatMessagePacket : OutgoingPacket
    {
        private MStream Stream;

        public ChatMessagePacket(string text)
        {
            Stream = new MStream();
            Stream.WriteByte(0x03);
            Stream.Write(text);
        }

        public override byte GetId()
        {
            return 0x03;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
