namespace Minecraft.World
{
    public class ChunkColumn
    {
        private readonly Chunk[] Chunks;
        private readonly BiomeData Biome;

        public ChunkColumn()
        {
            Chunks = new Chunk[16];
            Biome = new BiomeData();
        }

        public void Unpack(byte[] buffer, ushort mask1, ushort mask2, bool skylight = true)
        {
            // In the protocol, each section is packed sequentially (i.e. attributes
            // pertaining to the same chunk are *not* grouped)
            UnpackSection(buffer, ChunkDataSection.BLOCK_DATA, mask1);
            UnpackSection(buffer, ChunkDataSection.BLOCK_META, mask1);
            UnpackSection(buffer, ChunkDataSection.LIGHT_BLOCK, mask1);
            if (skylight)
            {
                UnpackSection(buffer, ChunkDataSection.LIGHT_SKY, mask1);
            }
            UnpackSection(buffer, ChunkDataSection.BLOCK_ADD, mask2);
            Biome.Unpack(buffer);
        }

        public void UnpackSection(byte[] buffer, ChunkDataSection section, ushort mask)
        {
            // Iterate over the bitmask
            for (int i = 0; i < 16; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    if (Chunks[i] is null)
                    {
                        Chunks[i] = new Chunk();
                    }
                }
                switch(section)
                {
                    case ChunkDataSection.BLOCK_DATA:
                        Chunks[i].BlockData.Unpack(buffer);
                        break;
                    case ChunkDataSection.BLOCK_META:
                        Chunks[i].BlockMeta.Unpack(buffer);
                        break;
                    case ChunkDataSection.LIGHT_BLOCK:
                        Chunks[i].LightBlock.Unpack(buffer);
                        break;
                    case ChunkDataSection.LIGHT_SKY:
                        Chunks[i].LightSky.Unpack(buffer);
                        break;
                    case ChunkDataSection.BLOCK_ADD:
                        Chunks[i].BlockAdd.Unpack(buffer);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
