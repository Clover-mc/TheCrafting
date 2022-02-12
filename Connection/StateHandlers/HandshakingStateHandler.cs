using System;

using Minecraft.Entities;
using Minecraft.Packets;
using Minecraft.Tools;

namespace Minecraft.Connection.StateHandlers
{
    internal class HandshakingStateHandler : StateHandler
    {
        public static HandshakingStateHandler Instance = new HandshakingStateHandler();
        internal override void Handle(Player player, byte[] packet)
        {
            MStream stream = MStream.From(packet);
            if (stream.GetArray().Length >= 3 &&
                stream.GetArray()[0] == '\xFE' &&
                stream.GetArray()[1] == '\x01' &&
                stream.GetArray()[2] == '\xFA')
            {
                Console.Write("== Legacy Server List Ping (1.6)");


                /*MStream response = new MStream();
                response.WriteByte(0xFF);

                byte[] send = Encoding.BigEndianUnicode.GetBytes("§1\0127\01.16.5\0A Minecraft Server\00\020");
                
                int length = send.Length;
                response.Write((short)(length / 2));
                response.Write(send, 0, send.Length);

                
                Console.Write(BitConverter.ToString(response.GetArray()).Replace('-', ' '));

                player.GetConnection().SendRaw(response.GetArray());*/
                player.GetConnection().CloseConnection();
                return;
            }

            stream.ReadVarInt();
            int packetid = stream.ReadVarInt().ToInt();

            if (packetid == 0x00)
            {
                PacketHandshakingInHandshake send = new PacketHandshakingInHandshake(packet);
                if (send.GetNextState().ToInt() == 1)
                {
                    player.GetConnection().SetState(PlayerConnectionState.STATUS);
                }
                else if (send.GetNextState().ToInt() == 2)
                {
                    player.GetConnection().SetState(PlayerConnectionState.LOGIN);
                }
            }
        }
    }
}
