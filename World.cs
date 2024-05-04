using System.Collections.Generic;
using System.Linq;
using Minecraft.Entities;

namespace Minecraft;

public class World
{
    public Dimension Dimension { get; set; }
    public byte WorldHeight { get; set; } = 0xFF;
    public LevelType LevelType { get; set; }
    public string? Path { get; private set; }
    public Dictionary<uint, IEntity> Entities { get; private set; } = new();

    internal World(LevelType type, Dimension dimension)
    {
        LevelType = type;
        Dimension = dimension;
    }

    public uint RegisterEntity(IEntity entity)
    {
        uint newKey = 0;
        IEnumerable<uint> keys = Entities.Keys;
        for (uint i = 0; keys.Any() && i <= keys.Max() + 1; i++)
            if (!keys.Contains(i))
            {
                newKey = i;
                break;
            }

        entity.EntityId = newKey;

        Entities.Add(newKey, entity);

        return newKey;
    }

    public void UnregisterEntity(IEntity entity)
    {
        UnregisterEntity(entity.EntityId);
    }

    public void UnregisterEntity(uint entityId)
    {
        Entities.Remove(entityId);
    }
}
