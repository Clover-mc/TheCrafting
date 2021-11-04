namespace Minecraft.Packets
{
    public class PlayerLookPacket : OutgoingPacket
    {
        private MStream Stream;

        public PlayerLookPacket(float yaw, float pitch, bool on_ground)
        {
            Stream = new MStream();
            Stream.WriteByte(0x0C);
            Stream.Write(yaw);
            Stream.Write(pitch);
            Stream.Write(on_ground);
        }

        public override byte GetId()
        {
            return 0x0C;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
