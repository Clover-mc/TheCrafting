using System;
using System.Net.Sockets;

namespace Minecraft.Events
{
    public class ServerListPingEvent : EventArgs
    {
        public Socket Client { get; }
        public string Description { get; set; } = string.Empty;

        internal ServerListPingEvent(Socket client)
            => Client = client;
    }
}
