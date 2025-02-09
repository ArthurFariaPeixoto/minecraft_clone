using System.Numerics;
using UnityEngine;

public class WaterLayerHandler : BlockLayerHandler
{
    public int waterLevel = 1;
    protected override bool TryHandling(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (position.y > surfaceHeightNoise && position.y <= waterLevel)
        {
            Chunk.SetBlock(chunkData, position, BlockType.Water);
            if (position.y == surfaceHeightNoise + 1 && position.y <= waterLevel + 1)
            {
                // position.y = surfaceHeightNoise;
                Chunk.SetBlock(chunkData, position, BlockType.Sand);
            }
            return true;
        }
        return false;
    }
}
