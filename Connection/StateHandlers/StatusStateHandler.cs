using Minecraft.Entities;
using Minecraft.Packets;
using Minecraft.Tools;

namespace Minecraft.Connection.StateHandlers
{
    internal class StatusStateHandler : StateHandler
    {
        public static StatusStateHandler Instance = new StatusStateHandler();
        internal override void Handle(Player player, byte[] packet)
        {
            MStream stream = MStream.From(packet);
            stream.ReadVarInt();
            VarInt id = stream.ReadVarInt();

            // Request Packet
            if (id.ToInt() == 0x00)
            {
                player.SendPacket(new PacketStatusOutServerListPing());
            }
            // Ping Packet
            else if (id.ToInt() == 0x01)
            {
                player.GetConnection().SendRaw(stream.GetArray());
                player.GetConnection().CloseConnection();
            }
        }
    }
}
