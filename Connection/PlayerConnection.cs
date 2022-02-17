using System;
using System.Linq;
using System.Net.Sockets;

using Minecraft.Packets;

namespace Minecraft
{
    public class PlayerConnection
    {
        private readonly Socket Client;
        private PlayerConnectionState State;
        private bool Connected;

        internal PlayerConnection(Socket client)
        {
            Client = client;
            Connected = Client.Connected;
            SetState(PlayerConnectionState.HANDSHAKING);
        }

        public void SendPacket(Packet packet)
        {
            byte[] packet_raw = packet.GetRaw();
            SendRaw(packet_raw);
        }

        public void CloseConnection()
        {
            Client.Disconnect(false);
            Connected = false;
        }

        public PlayerConnectionState GetState()
        {
            return State;
        }

        internal void SetState(PlayerConnectionState state)
        {
            State = state;
        }

        internal byte[] Receive()
        {
            byte[] array = new byte[1024];
            int length = Client.Receive(array);

            return Enumerable.ToArray(array.Take(length));
        }

        public void SendRaw(byte[] message)
        {
            if (MinecraftServer.GetInstance().GetLogOutgoing()) Console.WriteLine("\nBINARY(HEX): " + BitConverter.ToString(message).Replace('-', ' '));
            Client.Send(message);
        }

        public bool IsConnected()
        {
            return Connected;
        }
    }
}
