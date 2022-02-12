namespace Minecraft.Packets.Id
{
    public enum LoginOutId : int
    {
        DISCONNECT_LOGIN = 0x00,
        ENCRYPTION_REQUEST = 0x01,
        LOGIN_SUCCESS = 0x02,
        SET_COMPRESSION = 0x03,
        LOGIN_PLUGIN_REQUEST = 0x04
    }
}
