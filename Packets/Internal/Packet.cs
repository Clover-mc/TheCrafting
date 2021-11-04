namespace Minecraft.Packets
{
    public abstract class Packet
    {
        public abstract byte GetId();

        internal abstract byte[] GetRaw();
    }
}
