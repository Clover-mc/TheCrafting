namespace Minecraft.Packets
{
    public class PlayerPositionPacket : OutgoingPacket
    {
        private MStream Stream;

        public PlayerPositionPacket(double x, double y, double z, double stance, bool on_ground)
        {
            Stream = new MStream();
            Stream.WriteByte(0x0B);
            Stream.Write(x);
            Stream.Write(y);
            Stream.Write(stance);
            Stream.Write(z);
            Stream.Write(on_ground);
        }

        public override byte GetId()
        {
            return 0x0B;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
