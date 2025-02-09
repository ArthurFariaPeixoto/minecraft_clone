using System;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{

    public int waterThreshold = 10;
    public NoiseData biomeNoiseSettings;

    public BlockLayerHandler startLayerHandler;

    public ChunkData ProcessChunkColumn(ChunkData data, int x, int z, Vector2Int mapSeedOffset)
    {
        biomeNoiseSettings.worldOffset = mapSeedOffset;
        int groundPosition = GetSurfaceHeightNoise(data.worldPosition.x + x, data.worldPosition.z + z, data.chunkHeight);
        for (int y = 0; y < data.chunkHeight; y++)
        {
            startLayerHandler.Handle(data, new Vector3Int(x, y, z), groundPosition, mapSeedOffset);

            // BlockType voxelType = BlockType.Dirt;
            // if (y > groundPosition)
            // {
            //     if (y < waterThreshold)
            //     {
            //         voxelType = BlockType.Water;
            //     }
            //     else
            //     {
            //         voxelType = BlockType.Air;
            //     }

            // }
            // else if (y == groundPosition && y < waterThreshold)
            // {
            //     voxelType = BlockType.Sand;
            // }
            // else if (y == groundPosition)
            // {
            //     voxelType = BlockType.Grass_Dirt;
            // }

            // Chunk.SetBlock(data, new Vector3Int(x, y, z), voxelType);
        }
        return data;
    }

    private int GetSurfaceHeightNoise(int x, int z, int chunkHeight)
    {
        float terrainHeight = CustomNoise.OctavePerlin(x, z, biomeNoiseSettings);
        terrainHeight = CustomNoise.Redistribution(terrainHeight, biomeNoiseSettings);
        int surfaceHeight = CustomNoise.RemapValueSecondaryToInt(terrainHeight, 0, chunkHeight);
        return surfaceHeight;
    }
}
