using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Minecraft.Network.Packets;
using Serilog;

namespace Minecraft.Network
{
    public class PlayerConnection
    {
        private readonly Socket Client;

        public MinecraftServer Server { get; private set; }
        public bool Connected => Client.Connected;
        public string Nickname { get; internal set; }
        public bool IsPlayer { get; internal set; }

        internal PlayerConnection(MinecraftServer server, Socket client)
        {
            Server = server;
            Client = client;
            Nickname = string.Empty;
        }

        public int SendPacket(IPacket packet)
        {
            byte[] packetRaw = packet.Raw.ToArray();

            if (Server.Settings.ShowOutgoing)
            {
                Log.Debug("Outgoing Packet (to {Receiver}): {Packet}", Client.RemoteEndPoint, BitConverter.ToString(packetRaw).Replace('-', ' '));
            }

            return Client.Send(packetRaw);
        }

        public Task<int> SendPacketAsync(IPacket packet)
        {
            byte[] packetRaw = packet.Raw.ToArray();

            if (Server.Settings.ShowOutgoing)
            {
                Log.Debug("Outgoing Async Packet (to {Receiver}): {Packet}", Client.RemoteEndPoint, BitConverter.ToString(packetRaw).Replace('-', ' '));
            }

            return Client.SendAsync(packetRaw, SocketFlags.None);
        }

        public void Close()
        {
            Log.Debug("Disconnected: " + Client.RemoteEndPoint);

            Server._connections.RemoveAll(connection => connection.Player.Connection == this);
            Client.Close();

            if (IsPlayer)
            {
                Log.Information("[WORLD] " + Nickname + " left the game");
                Server.BroadcastMessage($"{ChatColor.Yellow}{Nickname} left the game");
            }
        }

        internal byte[] Receive()
        {
            if (Client.Available < 1)
                return Array.Empty<byte>();

            byte[] bytes = new byte[Client.Available];
            int received = 0;

            for (int i = 0; Client.Available > 0; i++)
            {
                received += Client.Receive(bytes, received, Math.Min(Client.Available, Client.ReceiveBufferSize), SocketFlags.None);
            }

            return bytes;
        }
    }
}
