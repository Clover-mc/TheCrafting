using System.Text;
using System.Text.Json;

namespace Minecraft.Tools
{
    public static class Json
    {
        public static string JsonToString(object obj)
        {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
