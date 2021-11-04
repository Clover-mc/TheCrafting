namespace Minecraft.Packets
{
    public class LoginRequestPacket : OutgoingPacket
    {
        private MStream Stream;

        public LoginRequestPacket(int entityID, string level_type, Gamemode gamemode, Dimension dimension, Difficulty difficulty, byte max_players)
        {
            Stream = new MStream();
            Stream.WriteByte(0x01);
            Stream.Write(entityID);
            Stream.Write(level_type);
            Stream.Write((byte)gamemode);
            Stream.Write((byte)dimension);
            Stream.Write((byte)difficulty);
            Stream.WriteByte(0);
            Stream.Write(max_players);
        }

        public override byte GetId()
        {
            return 0x01;
        }

        internal override byte[] GetRaw()
        {
            return Stream.GetArray();
        }
    }
}
