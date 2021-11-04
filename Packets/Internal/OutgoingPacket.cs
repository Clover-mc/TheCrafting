namespace Minecraft.Packets
{
    public abstract class OutgoingPacket : Packet
    {
        public abstract override byte GetId();

        internal abstract override byte[] GetRaw();
    }
}
