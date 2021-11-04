namespace Minecraft.Packets
{
    public class SpawnPositionPacket : OutgoingPacket
    {
        private MStream Stream;

        public SpawnPositionPacket(int x, int y, int z)
        {
            Stream = new MStream();
            Stream.WriteByte(0x06);
            Stream.Write(x);
            Stream.Write(y);
            Stream.Write(z);
        }

        public override byte GetId()
        {
            return 0x06;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
