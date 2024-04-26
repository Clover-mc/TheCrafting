// Latest version of protocol specification for 1.4.7 https://wiki.vg/index.php?title=Protocol&oldid=1037
// It's here because on their site (https://wiki.vg/Protocol_version_numbers) for versions from 12w30c to 1.6.1 no longer valid (why they deleted pages? and why they cannot update links for newer one?)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Minecraft.Commands;
using Minecraft.Configs;
using Minecraft.Entities;
using Minecraft.Network;
using Minecraft.Network.Packets;
using Minecraft.Network.Packets.Receivers;
using Minecraft.Tools;
using Serilog;

namespace Minecraft
{
    public partial class MinecraftServer
    {
        public DateTime StartTime { get; private set; }
        public ServerSettings Settings { get; private set; } = new();
        public bool Enabled { get; private set; }

        public IReadOnlyList<ConnectionHandler> Connections { get => _connections.AsReadOnly(); }
        public IReadOnlyList<Player> Players
        {
            get => Worlds.SelectMany(w => w.Entities
                    .Select(kv => kv.Value)
                    .OfType<Player>()
                    .Where(player => player.Connection?.Connected == true))
                .ToList()
                .AsReadOnly();
        }

        public FilePaths Files { get; } = new();
        public ConsolePlayer ConsolePlayer { get; } = new();

        #region Internal (Dirty) stuff
        public IntervalStorage Interval { get; } = new();
        public ConfigManager Config { get; } = new("server.properties.json");
        public PacketReceiverStorage ReceiverStorage { get; } = new();
        #endregion

        public CommandsHandler Commands { get; } = new();
        public List<World> Worlds { get; } = new();
        public TabListHandler TabList { get; }

        internal List<ConnectionHandler> _connections = new();

        public MinecraftServer()
        {
            TabList  = new TabListHandler(this);
        }

        public void Start(ServerSettings settings)
        {
            InitVariables(settings);

            Worlds.Add(new World(LevelType.Flat) { Dimension = Dimension.OVERWORLD });

            var listener = new TcpListener(IPAddress.Any, Config.Port);
            listener.Start();

            Enabled = true;
            TabList.Enabled = true;

            ListenClients(listener);
        }

        public void Stop()
        {
            Log.Information("Stopping server...");
            TabList.Enabled = false;

            foreach (var player in Players)
            {
                player.Disconnect("Server shutdown.");
            }

            Enabled = false;
        }

        public void BroadcastMessage(string message)
        {
            foreach (Player player in Players)
                if (player.Connection?.Connected == true)
                    player.SendMessage(message);
        }

        private void InitVariables(ServerSettings settings)
        {
            StartTime = DateTime.Now;
            Settings = settings;

            Files.ArchiveLatestLog();

            RegisterCommands();
            RegisterPacketReceivers();
        }

        private void ListenClients(TcpListener listener)
        {
            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                Log.Information("Ready for connection (" + Math.Round((DateTime.Now - StartTime).TotalSeconds, 2).ToString().Replace(',', '.') + "s)");
                while (Enabled)
                {
                    while (!listener.Pending())
                    {
                        if (!Enabled)
                        {
                            return;
                        }
                    }

                    // Start for listening connections
                    TcpClient client = listener.AcceptTcpClient();

                    // Create new task for each connected player
                    Task.Run(() =>
                    {
                        try
                        {
                            Log.Debug("Connection from: " + client.Client.RemoteEndPoint);
                            
                            // Start handling packets and connection
                            var handler = new ConnectionHandler(this, client);
                            _connections.Add(handler);
                            handler.Handle();
                        }
                        catch (Exception e)
                        {
                            // Ouch.
                            Log.Error(e, "Connection Handler Error!");
                        }
                    });
                }

            }
            catch (Exception e)
            {
                File.WriteAllText("crash.log", e.ToString());

                Log.Fatal(e, "Fatal error occurred!");
                Log.Information("Press any key to continue . . .");
                Console.Read();
            }
        }

        void RegisterCommands()
        {
            Commands.RegisterCommand<HelpCommand>("help");
            Commands.RegisterCommand<TeleportCommand>("teleport");
            Commands.RegisterCommand<DisplayNameCommand>("displayname");
            Commands.RegisterCommand<StopCommand>("stop");
        }

        void RegisterPacketReceivers()
        {
            ReceiverStorage.Add(0x00, new KeepalivePacketReceiver());
            ReceiverStorage.Add(0x02, new HandshakePacketReceiver());
            ReceiverStorage.Add(0x03, new ChatMessagePacketReceiver());
            ReceiverStorage.Add(0x0A, new PlayerPacketReceiver());
            ReceiverStorage.Add(0xFE, new ServerListPingPacketReceiver());
            ReceiverStorage.Add(0xFF, new DisconnectKickPacketReceiver());
        }
    }
}
