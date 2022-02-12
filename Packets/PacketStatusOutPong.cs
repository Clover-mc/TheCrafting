using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class PacketStatusOutPong : Packet
    {
        private readonly MStream Stream;
        private readonly long Payload;

        public PacketStatusOutPong(long payload)
        {
            Payload = payload;

            Stream = new MStream();
            Stream.Write(new VarInt(GetId()));
            Stream.Write(Payload);
            Stream.PrefixWith(new VarInt(Stream.GetArray().Length).ToPackedArray());
        }

        public override int GetId()
        {
            return (int)Id.StatusOutId.PONG;
        }

        public override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
