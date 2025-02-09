using UnityEngine;

public class NewMonoBehaviourScript : BlockLayerHandler
{
    public BlockType undergroundBlockType;

    protected override bool TryHandling(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (position.y < surfaceHeightNoise)
        {
            Chunk.SetBlock(chunkData, position, undergroundBlockType);
            return true;
        }
        return false;
    }
}
