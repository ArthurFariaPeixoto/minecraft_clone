using UnityEngine;
public class ChunkData
{
    public BlockType[] blocks;
    public int chunkSize = 32;
    public int chunkHeight = 32;
    public World worldReference;
    public Vector3Int worldPosition;

    public bool modifiedByPlayer = false;

    public ChunkData(int chunkSize, int chunkHeight, World world, Vector3Int worldPosition)
    {
        this.chunkSize = chunkSize;
        this.chunkHeight = chunkHeight;
        this.worldReference = world;
        this.worldPosition = worldPosition;
        blocks = new BlockType[chunkSize * chunkHeight * chunkSize];
    }

}