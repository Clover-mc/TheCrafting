using System.Net.Sockets;

using Minecraft.Exceptions;
using Minecraft.Packets;

namespace Minecraft
{
    public class PlayerConnection
    {
        private Socket Client;
        private MinecraftServer Server;

        internal PlayerConnection(Socket client, MinecraftServer server)
        {
            Client = client;
            Server = server;
        }

        public void SendPacket(Packet packet)
        {
            if (packet is IncomingPacket)
            {
                throw new WrongPacketDirectionException(packet.GetId(), "Incoming", "Outgoing");
            }
            byte[] packet_raw = packet.GetRaw();

            if (Server.log_out) Console.WriteLine(BitConverter.ToString(packet_raw).Replace('-', ' '));

            Client.Send(packet_raw);
        }

        public void CloseConnection()
        {
            Client.Close();
        }
    }
}
