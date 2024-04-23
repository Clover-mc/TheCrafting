using Minecraft.Entities;
using Minecraft.Packets;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Minecraft
{
    public class ConnectionHandler
    {
        public bool Connected { get => Player.Connection?.Connected ?? false; }
        
        public bool IsPlayer
        {
            get => Player.Connection?.IsPlayer ?? true;
            internal set
            {
                if (Player.Connection is not null)
                {
                    Player.Connection.IsPlayer = value;
                }
            }
        }
        
        public Player Player { get; private set; }
        
        internal ulong TickId { get; set; }
        
        readonly MinecraftServer _server;
        readonly TcpClient _client;

        internal ConnectionHandler(MinecraftServer server, TcpClient client)
        {
            _server = server;
            _client = client;
            Player = new Player(new PlayerConnection(_server, _client.Client), string.Empty);
            IsPlayer = false;
        }

        internal void Handle()
        {
            while (Connected)
            {
                IEnumerable<byte> rawPacket = Player.ReceivePacket();

                if (!rawPacket.Any())
                    continue;

                if (_server.Settings.ShowIncoming &&
                    rawPacket.ElementAt(0) != 0x00 && rawPacket.ElementAt(0) != 0x0A &&
                    rawPacket.ElementAt(0) != 0x0B && rawPacket.ElementAt(0) != 0x0D &&
                    rawPacket.ElementAt(0) != 0x0C && rawPacket.ElementAt(0) != 0xC9)
                {
                    Log.Debug("Incoming Packet (from {Sender}): {Packet}", _client.Client.RemoteEndPoint, BitConverter.ToString(rawPacket.ToArray()).Replace('-', ' '));
                }

                while (rawPacket.Any()) rawPacket = Handle(rawPacket);
            }

            _server.Interval.Cancel(Player.KeepaliveIndex);
        }

        internal IEnumerable<byte> Handle(IEnumerable<byte> rawPacket)
        {
            try
            {
                if (_server.ReceiverStorage.TryGetValue(rawPacket.ElementAt(0), out IPacketReceiver? receiver))
                {
                    return receiver.Process(this, _server, rawPacket);
                }
            }
            catch(Exception e)
            {
                Player.Disconnect("Internal Server Error");
                Log.Error(e, "An error was thrown while processing packet! Caused by: {Nickname} (EID: {EntityId})", Player.Nickname, Player.EntityId);
            }

            return Array.Empty<byte>();
        }
    }
}
