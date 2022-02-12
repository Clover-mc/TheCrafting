using System.Collections.Generic;

namespace Minecraft.World
{
    public class WorldGeneratorSettings
    {
        public readonly long Seed;
        public readonly byte GenerateFeatures;
        public readonly Dictionary<string, Dimension> Dimensions;

        public WorldGeneratorSettings(long seed, byte generateFeatures)
        {
            Seed = seed;
            GenerateFeatures = generateFeatures;
            Dimensions = new Dictionary<string, Dimension>();
        }

        public Dimension? GetDimension(string name)
        {
            if (DimensionExists(name))
                return Dimensions[name];
            return null;
        }

        public bool DimensionExists(string name)
        {
            return Dimensions.ContainsKey(name);
        }

        public void AddDimension(string name, Dimension dimension)
        {
            Dimensions.Add(name, dimension);
        }
    }
}
