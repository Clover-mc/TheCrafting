using Minecraft.Entities;
using Minecraft.Network.Packets;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Minecraft.Network;

/// <summary>
/// Delegate for obtaining current list of players for tab list
/// </summary>
/// <param name="server">Current running server</param>
/// <param name="current">List of currently displaying players</param>
/// <returns>New value for <see cref="TabListHandler.Players"/> if <see langword="not"/> <see langword="null"/>, ignored otherwise</returns>
public delegate IEnumerable<TabListPlayer>? GetPlayersDelegate(MinecraftServer server, List<TabListPlayer> current);

public class TabListHandler
{
    public List<TabListPlayer> Players { get; } = new();

    public GetPlayersDelegate PlayersDelegate { get; set; } = GetPlayersList;

    bool _enabled = false;
    CancellationTokenSource _ctSource = new();
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (!Enabled && value != Enabled)
            {
                if (!_ctSource.TryReset())
                {
                    _ctSource = new();
                }

                Start(_ctSource.Token);
            }

            if (Enabled && value != Enabled)
            {
                _ctSource.Cancel();
            }

            _enabled = value;
        }
    }

    readonly MinecraftServer _server;

    internal TabListHandler(MinecraftServer server)
    {
        _server = server;
    }

    private void Start(CancellationToken token)
    {
        Task.Run(async () =>
        {
            while (Enabled)
            {
                token.ThrowIfCancellationRequested();

                try
                {
                    Update(PlayersDelegate.Invoke(_server, Players));
                }
                catch (Exception e)
                {
                    Log.Error(e, "Unable to update tab list!");
                }

                await Task.Delay(1000);
            }
        }, token);
    }

    /// <summary>
    /// Updates info about players and sends it to everyone
    /// </summary>
    public void Update(IEnumerable<TabListPlayer>? players)
    {
        // Clear list for clients
        IPacket[] packetBuffer = Players
            .Select(x => new PlayerListItemPacket(x.DisplayName, false, 0))
            .ToArray();

        foreach (Player player in _server.Players)
            if (player.Connection?.Connected == true)
                player.Connection.SendPacketsAsync(packetBuffer);

        var newPlayers = players?.ToList() ?? Players;

        // Update list
        Players.Clear();
        Players.AddRange(newPlayers);

        // Send new list to clients
        packetBuffer = Players
            .Select(x => new PlayerListItemPacket(x.DisplayName, x.IsOnline, x.Ping))
            .ToArray();

        foreach (Player player in _server.Players)
            if (player.Connection?.Connected == true)
                player.Connection.SendPacketsAsync(packetBuffer);
    }

    public static IEnumerable<TabListPlayer>? GetPlayersList(MinecraftServer server, List<TabListPlayer> current)
    {
        var currentCopy = new List<TabListPlayer>(current);
        var players = server.Players.ToList();

        // Remove disconnected players
        foreach (var tabEntry in current)
        {
            var index = players.FindIndex(x => x.Nickname.Equals(tabEntry.Nickname, StringComparison.OrdinalIgnoreCase));
            if (index == -1)
            {
                currentCopy.RemoveAll(x => x.Nickname.Equals(tabEntry.Nickname, StringComparison.OrdinalIgnoreCase));
            }
        }

        // Add newly connected players
        foreach (var player in players)
        {
            var index = currentCopy.FindIndex(x => x.Nickname.Equals(player.Nickname, StringComparison.OrdinalIgnoreCase));
            if (index == -1)
            {
                currentCopy.Add(new(player.Nickname)
                {
                    DisplayName = player.DisplayName,
                    Dummy = false
                });
            }
        }

        for (int i = 0; i < currentCopy.Count; i++)
        {
            var entry = currentCopy[i];
            entry.DisplayName = players.First(x => x.Nickname.Equals(entry.Nickname, StringComparison.OrdinalIgnoreCase)).DisplayName;
            currentCopy[i] = entry;
        }

        currentCopy.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.DisplayName, y.DisplayName));

        return currentCopy;
    }
}
