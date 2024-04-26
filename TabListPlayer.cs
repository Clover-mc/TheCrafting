namespace Minecraft;

public struct TabListPlayer
{
    public readonly string Nickname;

    public string DisplayName;
    public short Ping;
    public bool IsOnline;

    internal bool Dummy = true;

    public TabListPlayer(string nickname)
    {
        DisplayName = Nickname = nickname;
        IsOnline = true;
        Dummy = true;
    }
}
