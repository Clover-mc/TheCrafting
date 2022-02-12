using System;

using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class PacketLoginOutLoginSuccess : Packet
    {
        private readonly MStream Stream;
        private readonly Guid UUID;
        private readonly string Nickname;

        public PacketLoginOutLoginSuccess(Guid uuid, string nickname)
        {
            UUID = uuid;
            Nickname = nickname;

            Stream = new MStream();
            Stream.Write(new VarInt(GetId()));
            Stream.Write(UUID);
            Stream.Write(Nickname);

            Stream.PrefixWith(new VarInt(Stream.GetArray().Length).ToPackedArray());
        }

        public Guid GetUUID()
        {
            return UUID;
        }

        public string GetNickname()
        {
            return Nickname;
        }

        public override int GetId()
        {
            return (int)Id.LoginOutId.LOGIN_SUCCESS;
        }

        public override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
