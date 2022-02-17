using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using Minecraft.Connection.StateHandlers;
using Minecraft.Entities;
using Minecraft.Tools;

namespace Minecraft
{
    internal class ConnectionHandler
    {
        internal static void Handle(TcpClient client)
        {
            Player player = new Player(new PlayerConnection(client.Client), "");
            try
            {
                while (player.GetConnection().IsConnected())
                {
                    byte[] packet_raw = player.ReceivePacket();
                    if (packet_raw.Length == 0) continue;

                    if (MinecraftServer.GetInstance().GetLogIncoming())
                    {
                        Console.WriteLine("==New incoming packet==");
                        Console.WriteLine("RAW: " + BitConverter.ToString(packet_raw, 0, packet_raw.Length).Replace('-', ' '));
                        Console.WriteLine("STRING: " + Encoding.UTF8.GetString(packet_raw));
                        Console.WriteLine("=======================");
                    }

                    VarInt packetLength;
                    int offset = 0;
                    byte[] sliced = packet_raw;
                    while (true)
                    {
                        packetLength = VarInt.From(sliced.Take(3).ToArray());
                        int sliced_length = packetLength.ToPackedArray().Length + packetLength.ToInt();

                        sliced = packet_raw.Skip(offset).Take(sliced_length).ToArray();

                        if (offset > packet_raw.Length) break;
                        if (offset == packet_raw.Length - sliced_length || packetLength.ToInt() >= packet_raw.Length)
                        {
                            HandlePacket(player, sliced);
                            break;
                        }
                        else
                        {
                            HandlePacket(player, sliced);
                            offset += sliced_length;
                        }
                    };
                }
            }
            catch(Exception e)
            {
                ConsoleOutputWrapper.WriteError(e);
                player.GetConnection().CloseConnection();
            }
        }

        private static void HandlePacket(Player player, byte[] packet)
        {
            switch (player.GetConnection().GetState())
            {
                case PlayerConnectionState.HANDSHAKING:
                    HandshakingStateHandler.Instance.Handle(player, packet);
                    break;
                case PlayerConnectionState.STATUS:
                    StatusStateHandler.Instance.Handle(player, packet);
                    break;
                case PlayerConnectionState.LOGIN:
                    LoginStateHandler.Instance.Handle(player, packet);
                    break;
                case PlayerConnectionState.PLAY:
                    break;
                default:
                    break;
            }
        }
    }
}
