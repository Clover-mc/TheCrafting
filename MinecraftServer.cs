using System;
using System.Collections.Generic;
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
    public class MinecraftServer : IServer
    {
        private DateTime StartTime;
        private bool LogIncoming = false;
        private bool LogOutgoing = false;
        private bool DisableConsole = false;
        private bool Enabled = false;
        private bool Launched = false;

        private readonly static MinecraftServer Instance = new MinecraftServer();

        private readonly static FilesHandler Files = new FilesHandler();
        private readonly static ConsoleInputHandler ConsoleInputHandler = new ConsoleInputHandler();
        private readonly ConfigManager CfgManager = new ConfigManager("server.properties");

        //public readonly byte[] PublicKey = Array.Empty<byte>();
        //internal readonly RSA Rsa;

        private readonly List<World.World> Worlds = new List<World.World>();

        private MinecraftServer()
        {
            //Rsa = RSA.Create(1024);
            //PublicKey = Rsa.ExportRSAPublicKey();
        }

        public void Start(string[] args)
        {
            StartTime = DateTime.Now;
            LogIncoming = args.Contains("log-incoming");
            LogOutgoing = args.Contains("log-outgoing");
            DisableConsole = args.Contains("noconsole");

            Console.SetOut(new ConsoleOutputWrapper());
            ConsoleOutputWrapper.Initialize();
            Thread.CurrentThread.Name = "main";

            Files.Initialize();

            Console.WriteLine("Reading/Creating World");
            Worlds.Add(new World.World(CfgManager.GetWorldName(), true));

            if (!DisableConsole) ConsoleInputHandler.Enable();

            TcpListener listener = new TcpListener(IPAddress.Any, CfgManager.GetPort());
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
                ConsoleOutputWrapper.GetWriter().WriteLine("\nOh, It's looks like server was crashed due to error!");
                ConsoleOutputWrapper.GetWriter().WriteLine("\nPress any key to continue . . .");
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

        public bool GetLogIncoming()
        {
            return LogIncoming;
        }
        public bool GetLogOutgoing()
        {
            return LogOutgoing;
        }

        public ConfigManager GetConfigManager()
        {
            return CfgManager;
        }
        
        public static MinecraftServer GetInstance()
        {
            return Instance;
        }

        public bool IsLaunched()
        {
            return Launched;
        }

        public List<World.World> GetWorlds()
        {
            return Worlds;
        }

        public static FilesHandler GetFilesHandler()
        {
            return Files;
        }

        public static ConsoleInputHandler GetConsoleInput()
        {
            return ConsoleInputHandler;
        }

        public static int Main(string[] args)
        {
            if (!GetInstance().IsLaunched())
            {
                GetInstance().Launched = true;
                GetInstance().Start(args);
                return 0;
            }
            return 1;
        }
    }
}