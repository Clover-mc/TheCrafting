using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
//using System.Security.Cryptography;

using Minecraft.External;
using Minecraft.Tools;

namespace Minecraft
{
    public class MinecraftServer
    {
        public static bool LogIncoming { get; private set; } = false;
        public static bool LogOutgoing { get; private set; } = false;
        public static bool DisableConsole { get; private set; } = false;
        public static string MOTD { get; private set; } = "";
        internal static MinecraftServer? Instance { get; private set; }

        public readonly static FilesHandler Files = new FilesHandler();
        public readonly static ConsoleInputHandler ConsoleInputHandler = new ConsoleInputHandler();

        public bool Enabled { get; private set; } = false;
        private bool Launched = false;
        private readonly ConfigManager CfgManager;

        //public readonly byte[] PublicKey = Array.Empty<byte>();
        //internal readonly RSA Rsa;

        public readonly DateTime StartTime;

        public readonly World.World World;

        private MinecraftServer(bool log_in = false, bool log_out = false, bool disable_console = false)
        {
            StartTime = DateTime.Now;
            Console.Title = "TheCrafting 1.16.5 | Plugins available: 0 | Plugins enabled: 0";
            Instance = this;
            Console.SetOut(new ConsoleOutputWrapper());
            ConsoleOutputWrapper.Initialize();
            Thread.CurrentThread.Name = "main";
            LogIncoming = log_in;
            LogOutgoing = log_out;
            DisableConsole = disable_console;

            Files.Initialize();

            Console.WriteLine("Reading config...");
            CfgManager = new ConfigManager("server.properties");
            MOTD = CfgManager.MOTD;

            Console.WriteLine("Reading/Creating World");
            World = new World.World(CfgManager.WorldName, true);


            if (!DisableConsole) ConsoleInputHandler.Enable();

            //Rsa = RSA.Create(1024);
            //PublicKey = Rsa.ExportRSAPublicKey();
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, CfgManager.PORT);
            Enabled = true;
            listener.Start();

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                Console.WriteLine("Ready for connection (" + Math.Round((DateTime.Now - StartTime).TotalSeconds, 2) + "s)");
                while (Enabled)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    new Task(() =>
                    {
                        try
                        {
                            Thread.CurrentThread.Name = "Network Thread";
                            ConnectionHandler.Handle(client);
                        }
                        catch(Exception e)
                        {
                            ConsoleOutputWrapper.Write(ConsoleColor.Red, "Internal Code Error: " + e.ToString(), ConsoleOutputLevel.LogLevel.ERROR);
                        }
                    }).Start();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ConsoleOutputWrapper.Writer.WriteLine("\nOh, It's looks like server was crashed due to error!");
                ConsoleOutputWrapper.Writer.WriteLine("\nPress any key to continue . . .");
                Console.Read();
            }

        }

        public void Stop()
        {
            Enabled = false;
            Console.Write("Stopping server...");
            Files.Stop();
            Process.GetCurrentProcess().Kill();
        }

        public static int Main(string[] args)
        {
            if (Instance is null || !Instance.Launched)
            {
                MinecraftServer server = new MinecraftServer(args.Contains("log-incoming"), args.Contains("log-outgoing"), args.Contains("noconsole"));
                if (Instance is not null) Instance.Launched = true;
                server.Start();
                return 0;
            }
            return 1;
        }
    }
}