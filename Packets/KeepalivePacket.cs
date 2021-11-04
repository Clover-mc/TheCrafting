

namespace Minecraft.Packets
{
    public class KeepalivePacket : OutgoingPacket
    {
        private MStream Stream;

        public KeepalivePacket()
        {
            Stream = new MStream();
            Random random = new Random();
            Stream.WriteByte(0x00);
            Stream.Write(random.Next(100000));
        }

        public override byte GetId()
        {
            return 0x00;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
