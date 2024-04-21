using System.Collections.Generic;

using Minecraft.Entities;

namespace Minecraft.Commands;

public interface ICommand
{
    public bool OnCommand(IEntity sender, string label, string raw, string[] args);

    public IList<string>? OnTabComplete(IEntity sender, string label, string raw, string[] args);
}
