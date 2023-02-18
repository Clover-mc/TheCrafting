using System;
using System.Linq;

namespace Minecraft;

/// <summary>
/// Class for entry point of simple launcher
/// </summary>
internal class Program
{
    /// <summary>
    /// Entry point of simple launcher
    /// </summary>
    /// <param name="args">Launch args</param>
    /// <returns>Result code</returns>
    static int Main(string[] args)
    {
        MinecraftServer server = new MinecraftServer();

        server.Start(ArgsToSettings(args));

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
            ShowOutgoing = Contains(args, "-O", "--out", "--outgoing"),
            DisableConsoleInput = Contains(args, "-D", "--disable-console-input")
        };
    }
}
