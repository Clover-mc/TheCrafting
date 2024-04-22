using System;

using Minecraft.Tools;

namespace Minecraft.Entities
{
    public class ConsolePlayer : Player
    {
        private MinecraftServer Server;

        internal ConsolePlayer(MinecraftServer server) : base(null, "") => Server = server;

        public override void SendMessage(string message)
        {
            ConsoleWrapper.ConsoleWriter.AllocLine();
            PrintColorizedString(message);
        }

        public void PrintColorizedString(string chatColoredString)
        {
            if (chatColoredString.Contains(ChatColor.Character))
            {
                for (int i = 0; i < chatColoredString.Length;)
                {
                    if (chatColoredString[i] != ChatColor.Character)
                    {
                        Console.Write(chatColoredString[i]);
                        i++;
                        continue;
                    }

                    int chatColor = Enumeration.FromDisplayName<ChatColor>(chatColoredString[i..(i + 2)]).Id;
                    chatColoredString = chatColoredString.Remove(i, chatColoredString.Length >= i + 1 ? 2 : 1);

                    if ((chatColor < 0 || chatColor > 15) && chatColor != 21) continue;

                    if (chatColor == 21) ConsoleWrapper.ConsoleWriter.ResetColor();
                    else Console.ForegroundColor = (ConsoleColor)chatColor;
                }
            }
            else Console.Write(chatColoredString);
            ConsoleWrapper.ConsoleWriter.ResetColor();
        }
    }
}
