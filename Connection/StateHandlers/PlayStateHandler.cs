using System;
//using System.Security.Cryptography;

using Minecraft.Entities;
using Minecraft.Packets.Id;
using Minecraft.Tools;

namespace Minecraft.Connection.StateHandlers
{
    internal class PlayStateHandler : StateHandler
    {
        public static PlayStateHandler Instance = new PlayStateHandler();
        internal override void Handle(Player player, byte[] packet)
        {
            MStream stream = MStream.From(packet);
            stream.ReadVarInt();
            PlayInId id = (PlayInId)stream.ReadVarInt().ToInt();

            Console.WriteLine("Woa (" + (int)id + "): " + BitConverter.ToString(packet));
        }
    }
}
