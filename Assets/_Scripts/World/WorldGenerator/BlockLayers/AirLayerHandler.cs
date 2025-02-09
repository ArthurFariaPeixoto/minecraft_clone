using UnityEngine;

public class AirHandler : BlockLayerHandler
{
    protected override bool TryHandling(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (position.y > surfaceHeightNoise)
        {
            Chunk.SetBlock(chunkData, position, BlockType.Air);
            return true;
        }
        return false;
    }
}
