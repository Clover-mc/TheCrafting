// Latest version of protocol specification for 1.4.7 https://wiki.vg/index.php?title=Protocol&oldid=1037
// It's here because on their site (https://wiki.vg/Protocol_version_numbers) for versions from 12w30c to 1.6.1 no longer valid (why they deleted pages? and why they cannot update links for newer one?)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Minecraft.Commands;
using Minecraft.Entities;
using Minecraft.Packets;
using Minecraft.Packets.Receivers;
using Minecraft.Tools;

namespace Minecraft
{
    public class MinecraftServer
    {
        public DateTime StartTime { get; private set; }
        public ServerSettings Settings { get; private set; }
        public bool Enabled { get; private set; }

        public IReadOnlyList<ConnectionHandler> Connections { get => _connections.AsReadOnly(); }
        public IReadOnlyList<Player> Players { get => Worlds.SelectMany(w => w.Entities.Where(kv => kv.Value is Player && ((kv.Value as Player)?.Connection.Connected ?? false)).Select(kv => (Player)kv.Value)).ToList().AsReadOnly(); }

        #region Internal (Dirty) stuff
        public FilesHandler Files { get; private set; } = new FilesHandler();
        public IntervalStorage Interval { get; private set; } = new IntervalStorage();
        public ConfigManager Config { get; private set; } = new ConfigManager("server.properties.json");
        public PacketReceiverStorage ReceiverStorage { get; private set; } = new PacketReceiverStorage();
        public ConsoleWrapper ConWrapper { get; private set; }
        public ConsolePlayer ConsolePlayer { get; private set; }
        #endregion

        public CommandsHandler Commands { get; private set; }
        public List<World> Worlds { get; private set; }
        public TabListHandler TabList { get; private set; }

        internal List<ConnectionHandler> _connections = new List<ConnectionHandler>();

        public MinecraftServer()
        {
            ConWrapper = new ConsoleWrapper(this);
            ConsolePlayer = new ConsolePlayer(this);
            Commands = new CommandsHandler();
            TabList  = new TabListHandler(this) { Enabled = true };
            Settings = new ServerSettings();
            Worlds   = new List<World>();
        }

        public void Start(ServerSettings settings)
        {
            InitVariables(settings);

            Worlds.Add(new World(LevelType.FLAT) { Dimension = Dimension.OVERWORLD });

            TcpListener listener = new TcpListener(IPAddress.Any, Config.Port);
            listener.Start();
            Enabled = true;

            ListenClients(listener);
        }

        public void Stop()
        {
            Enabled = false;
            Console.Write("Stopping server...");
            Files.Stop();

            Console.In.Close();
            Console.Out.Close();
        }

        public void BroadcastMessage(string message)
        {
            foreach (Player player in Players)
                if (player.Connection.Connected)
                    player.SendMessage(message);
        }

        private void InitVariables(ServerSettings settings)
        {
            StartTime = DateTime.Now;
            Settings = settings;

            Console.SetOut(ConWrapper.Writer);
            Console.SetIn(ConWrapper);
            Thread.CurrentThread.Name = "Main";

            Files.Initialize();

            RegisterCommands();
            RegisterPackertReceivers();

            if (!Settings.DisableConsoleInput)
                ConWrapper.EnableInput();
        }

        private void ListenClients(TcpListener listener)
        {
            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                Console.WriteLine("Ready for connection (" + Math.Round((DateTime.Now - StartTime).TotalSeconds, 2).ToString().Replace(',', '.') + "s)");
                while (Enabled)
                {
                    // Start for listening connections
                    TcpClient client = listener.AcceptTcpClient();

                    // Create new task for each connected player
                    Task.Run(() =>
                    {
                        try
                        {
                            Thread.CurrentThread.Name = "Network Player Thread";
                            Console.WriteLine("Connection from: " + client.Client.RemoteEndPoint);
                            // Start handling packets and connection
                            ConnectionHandler handler = new ConnectionHandler(this, client);
                            _connections.Add(handler);
                            handler.Handle();
                        }
                        catch (Exception e)
                        {
                            // Whooops
                            ConsoleWrapper.ConsoleWriter.WriteError("Internal Code Error: " + e);
                        }
                    });
                }

            }
            catch (Exception e)
            {
                File.WriteAllText("crash.log", e.ToString());

                Console.WriteLine(e.ToString());
                ConsoleWrapper.ConsoleWriter.Writer.WriteLine("\nOh, It's looks like server was crashed due to error!");
                ConsoleWrapper.ConsoleWriter.Writer.WriteLine("\nPress any key to continue . . .");
                Console.Read();
            }
        }

        private void RegisterCommands()
        {
            Commands.RegisterCommand("help", new HelpCommand());
        }

        private void RegisterPackertReceivers()
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