namespace Minecraft.Packets
{
    public abstract class IncomingPacket : Packet
    {
        public abstract override byte GetId();

        internal abstract override byte[] GetRaw();
    }
}
