using System;

namespace Minecraft.World
{
    public class GameRule
    {
        private readonly string Name;
        private string Value;

        public GameRule(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetValue()
        {
            return Value;
        }

        public void SetValue(string value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is not null && obj is GameRule gr && gr.GetName() == GetName();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetName());
        }
    }
}
