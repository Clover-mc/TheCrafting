namespace Minecraft.Packets
{
    public class GeneralPacket : Packet
    {
        private byte id;
        private byte[] data;
        public GeneralPacket(byte id, byte[]? data) : base()
        {
            this.id = id;
            this.data = data is not null ? data : new byte[0] { };
        }

        public override byte GetId()
        {
            return id;
        }

        internal override byte[] GetRaw()
        {
            byte[] raw = new byte[data.Length + 1];
            raw[0] = id;
            data.CopyTo(raw, 1);
            return raw;
        }
    }
}
