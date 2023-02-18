using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Minecraft.Entities;
using Minecraft.Packets;
using Minecraft.Tools;

namespace Minecraft
{
    public class ConnectionHandler
    {
        public bool Connected { get => Player.Connection.Connected; }
        public bool IsPlayer { get => Player.Connection.IsPlayer; internal set => Player.Connection.IsPlayer = value; }
        public Player Player { get; private set; }
        internal ulong TickId { get; set; }
        private readonly MinecraftServer Server;

        internal ConnectionHandler(MinecraftServer server, TcpClient client)
        {
            Server = server;
            Player = new Player(new PlayerConnection(Server, client.Client), "");
            IsPlayer = false;
        }

        internal void Handle()
        {
            while (Connected)
            {
                IEnumerable<byte> rawPacket = Player.ReceivePacket();

                if (!rawPacket.Any())
                    continue;

                if (Server.Settings.ShowIncoming &&
                    rawPacket.ElementAt(0) != 0x00 && rawPacket.ElementAt(0) != 0x0A &&
                    rawPacket.ElementAt(0) != 0x0B && rawPacket.ElementAt(0) != 0x0D &&
                    rawPacket.ElementAt(0) != 0x0C && rawPacket.ElementAt(0) != 0xC9)
                {
                    Console.WriteLine("==New incoming packet==");
                    Console.WriteLine("RAW: " + BitConverter.ToString(rawPacket.ToArray(), 0, rawPacket.Count()).Replace('-', ' '));
                    Console.WriteLine("STRING: " + Encoding.UTF8.GetString(rawPacket.ToArray()));
                    Console.WriteLine("=======================");
                }

                while (rawPacket.Any()) rawPacket = Handle(rawPacket);
            }
            Server.Interval.Cancel(Player.KeepaliveIndex);
        }

        internal IEnumerable<byte> Handle(IEnumerable<byte> rawPacket)
        {
            try
            {
                if (Server.ReceiverStorage.TryGetValue(rawPacket.ElementAt(0), out IPacketReceiver? receiver))
                    return receiver.Process(this, Server, rawPacket);
            }
            catch(Exception e)
            {
                Player.Disconnect("Internal Server Error");
                ConsoleWrapper.ConsoleWriter.WriteError(e);
            }

            return Array.Empty<byte>();
        }
    }
}