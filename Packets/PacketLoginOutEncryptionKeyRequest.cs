using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class PacketLoginOutEncryptionKeyRequest : Packet
    {
        private readonly MStream Stream;
        private readonly byte[] PublicKey;
        private readonly byte[] VerifyToken;
        private readonly string ServerID;

        public PacketLoginOutEncryptionKeyRequest(byte[] publicKey, byte[] verifyToken, string serverID = "\0")
        {
            PublicKey = publicKey;
            VerifyToken = verifyToken;
            ServerID = serverID;

            Stream = new MStream();
            Stream.Write(new VarInt(GetId()));
            Stream.Write(ServerID);
            Stream.Write(new VarInt(PublicKey.Length));
            Stream.Write(PublicKey);
            Stream.Write(new VarInt(VerifyToken.Length));
            Stream.Write(VerifyToken);

            Stream.PrefixWith(new VarInt(Stream.GetArray().Length).ToPackedArray());
        }

        public byte[] GetPublicKey()
        {
            return PublicKey;
        }

        public byte[] GetVerifyToken()
        {
            return VerifyToken;
        }

        public string GetServerID()
        {
            return ServerID;
        }

        public override int GetId()
        {
            return (int)Id.LoginOutId.ENCRYPTION_REQUEST;
        }

        public override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
