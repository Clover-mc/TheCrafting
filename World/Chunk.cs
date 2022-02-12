namespace Minecraft.World
{
    public class Chunk
    {
        public ChunkData BlockData;
        public ChunkHalfData BlockMeta;
        public ChunkHalfData BlockAdd;
        public ChunkHalfData LightBlock;
        public ChunkHalfData LightSky;

        public Chunk()
        {
            BlockData = new ChunkData();
            BlockMeta = new ChunkHalfData();
            BlockAdd = new ChunkHalfData();  
            LightBlock = new ChunkHalfData();
            LightSky = new ChunkHalfData();
        }
    }
}
