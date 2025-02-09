using System;
using UnityEngine;

public abstract class BlockLayerHandler : MonoBehaviour
{
    [SerializeField]
    private BlockLayerHandler Next;

    public bool Handle(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise, Vector2Int mapSeedOffset)
    {
        if (TryHandling(chunkData, position, surfaceHeightNoise, mapSeedOffset)) return true;
        if (Next != null) return Next.Handle(chunkData, position, surfaceHeightNoise, mapSeedOffset);
        return false;
    }

    protected abstract bool TryHandling(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise, Vector2Int mapSeedOffset);


}
