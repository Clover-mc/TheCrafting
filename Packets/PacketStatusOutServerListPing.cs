using System.Dynamic;
using System.Text.Json;

using Minecraft.Tools;

namespace Minecraft.Packets
{
    public class PacketStatusOutServerListPing : Packet
    {
        public PacketStatusOutServerListPing() { }

        public override int GetId()
        {
            return (int)Id.StatusOutId.RESPONSE;
        }

        public override byte[] GetRaw()
        {

            dynamic answer = new ExpandoObject();
            answer.version = new ExpandoObject();
            answer.players = new ExpandoObject();
            answer.description = new ExpandoObject();

            answer.version.name = "1.16.5";
            answer.version.protocol = 754;

            answer.players.max = 20;
            answer.players.online = 0;

            answer.description = new Chat.Builder.TextComponent(MinecraftServer.GetInstance().GetConfigManager().GetMOTD(), false, false, false, false, false, "minecraft:alt", "yellow");

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(answer);

            MStream stream = new MStream();
            stream.Write(new VarInt(0).ToPackedArray());
            stream.Write(new VarInt(bytes.Length).ToPackedArray());
            stream.Write(bytes);

            byte[] length = new VarInt((int)stream.GetStream().Length).ToPackedArray();
            byte[] result = new byte[(int)stream.GetStream().Length + length.Length];

            length.CopyTo(result, 0);
            stream.GetArray().CopyTo(result, length.Length);

            return result;
        }
    }
}
