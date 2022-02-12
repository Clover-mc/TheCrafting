namespace Minecraft.Packets
{
    public abstract class Packet
    {
        public abstract int GetId();

        public abstract byte[] GetRaw();
    }
}
