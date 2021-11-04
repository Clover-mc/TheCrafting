namespace Minecraft.Exceptions
{
    public class WrongPacketDirectionException : Exception
    {
        public WrongPacketDirectionException() { }

        public WrongPacketDirectionException(byte id, string given_direction, string expected_direction) : base($"Packet with id {id} has direction \"{given_direction}\", expected \"{expected_direction}\"") { }

        public WrongPacketDirectionException(byte id, string given_direction, string expected_direction, Exception inner) : base($"Packet with id {id} has direction \"{given_direction}\", expected \"{expected_direction}\"", inner) { }
    }
}
