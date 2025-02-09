using UnityEngine;

public class SurfaceLayerHandler : BlockLayerHandler
{
    public BlockType surfaceBlockType;
    protected override bool TryHandling(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (position.y == surfaceHeightNoise)
        {
            Chunk.SetBlock(chunkData, position, surfaceBlockType);
            return true;
        }
        return false;
    }
}
