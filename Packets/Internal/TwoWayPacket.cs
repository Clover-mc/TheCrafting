namespace Minecraft.Packets
{
    public abstract class TwoWayPacket : Packet
    {
        public abstract byte GetIncomingId();
        public abstract byte GetOutgoingId();

        internal abstract override byte[] GetRaw();
    }
}
