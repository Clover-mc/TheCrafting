using System;
using System.Collections.Generic;

namespace Minecraft.World
{
    public class BossEvent
    {
        public string ID;
        public List<Guid> Players;
        public string Color;
        public bool CreateWorldFog;
        public bool DarkenScreen;
        public int Max;
        public int Value;
        public Chat.Builder.TextComponent Name;
        public string Overlay;
        public bool PlayBossMusic;
        public bool Visible;

        public BossEvent(string id, string color, int max, Chat.Builder.TextComponent name, bool visible)
        {
            ID = id;

            Players = new List<Guid>();
            Color = color;
            CreateWorldFog = false;
            DarkenScreen = false;
            Max = max;
            Value = 0;
            Name = name;
            Overlay = "progress";
            PlayBossMusic = false;
            Visible = visible;
        }
    }
}
