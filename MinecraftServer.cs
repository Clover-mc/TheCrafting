using System.Net;
using System.Net.Sockets;
using System.Text;

using Minecraft.External;
using Minecraft.Packets;

namespace Minecraft
{
    internal class MinecraftServer
    {
        public bool log_in = false;
        public bool log_out = false;
        private bool enabled = false;
        private ConfigManager CManager;

        private MinecraftServer(bool log_in = false, bool log_out = false)
        {
            this.log_in = log_in;
            this.log_out = log_out;
            Console.WriteLine("Reading config...");
            CManager = new ConfigManager("server.properties");
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, CManager.PORT);
            enabled = true;
            listener.Start();

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                Console.WriteLine("Ready for connection");
                while (enabled)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine($"New client ({client.Client.RemoteEndPoint})");

                    byte[] packet_raw = new byte[1024];
                    int length = client.Client.Receive(packet_raw);

                    PlayerConnection player = new PlayerConnection(client.Client, this);

                    if (log_in)
                    {
                        Console.WriteLine("==New incoming packet==");
                        Console.WriteLine("RAW: " + BitConverter.ToString(packet_raw, 0, length).Replace('-', ' '));
                        Console.WriteLine("STRING: " + Encoding.UTF8.GetString(packet_raw));
                    }

                    GeneralPacket packet = new GeneralPacket(packet_raw[0], Enumerable.ToArray(packet_raw.Skip(1)));

                    switch(packet.GetId())
                    {
                        case 0x02: // This will show text 'Downloading terrain' after connecting and "Logging in"
                            player.SendPacket(new LoginRequestPacket(0, "default", Gamemode.SURVIVAL, Dimension.OVERWORLD, Difficulty.NORMAL, 20));
                            player.SendPacket(new MapChunkBulkPacket(49, true, new byte[602112]));
                            player.SendPacket(new PlayerPositionPacket(0, 70, 0, 0, false));
                            player.SendPacket(new PlayerLookPacket(0, 0, false));
                            Task t = Task.Run(() =>
                            {
                                while(true)
                                {
                                    player.SendPacket(new PlayerPositionPacket(0, 256, 0, 0, true));
                                    player.SendPacket(new KeepalivePacket());
                                    player.SendPacket(new ChatMessagePacket(ChatColor.GOLD + ChatColor.ITALIC + "PogChamp"));
                                    Thread.Sleep(5000);
                                }
                            });
                            break;
                        case 0xFE:
                            //"A Custom " + ChatColor.DARK_PURPLE + "C" + ChatColor.GRAY + "#" + ChatColor.RESET + " Minecraft Server"
                            player.SendPacket(new ServerListPingPacket('1', 51, "1.4.7", CManager.MOTD, 0, 20));
                            player.CloseConnection();
                            break;
                        default:
                            player.SendPacket(new DisconnectKickPacket(ChatColor.GOLD + ChatColor.BOLD + "FUCK IT, EVOLUTION TIME"));
                            player.CloseConnection();
                            break;
                    }

                    //new Task((() => { HandleClientCommNew(client); })).Start(); //Task instead of Thread
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nOh, It's looks like server is crash due to error!");
            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void Stop()
        {
            enabled = false;
        }

        public static int Main(string[] args)
        {
            MinecraftServer server = new MinecraftServer(args.Contains("log-incoming"), args.Contains("log-outgoing"));
            server.Start();

            return 0;
        }
    }
}