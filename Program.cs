using Minecraft;
using Serilog;
using System;
using System.Linq;

internal class Program
{
    static int Main(string[] args)
    {
        var server = new MinecraftServer();

        // Control-C Handler
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            server.Stop();
        };

        server.Files.ArchiveLatestLog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(server.Files.LatestLog)
            .CreateLogger();

        server.Start(ArgsToSettings(args));

        Log.CloseAndFlush();

        return 0;
    }

    /// <summary>
    /// Converts launch args to <see cref="ServerSettings"/>
    /// </summary>
    /// <param name="args">Launch arguments</param>
    /// <returns>Parsed settings</returns>
    static ServerSettings ArgsToSettings(string[] args)
    {
        static bool Contains(string[] args, params string[] searchArgs)
        {
            foreach (string arg in searchArgs)
                if (args.Contains(arg))
                    return true;

            return false;
        }

        return new ServerSettings()
        {
            ShowIncoming = Contains(args, "-I", "--in", "--incoming"),
            ShowOutgoing = Contains(args, "-O", "--out", "--outgoing")
        };
    }
}
