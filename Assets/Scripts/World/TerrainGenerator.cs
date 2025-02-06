using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int mapSizeInChunks = 17;
    public int chunkSize = 32, chunkHeight = 32;
    public int waterThreshold = 10;
    public float noiseScale = 0.03f;
    public GameObject chunkPrefab;

    Dictionary<Vector3Int, ChunkData> chunkDataDictionary = new Dictionary<Vector3Int, ChunkData>();
    Dictionary<Vector3Int, ChunkRender> chunkDictionary = new Dictionary<Vector3Int, ChunkRender>();

    public void GenerateWorld()
    {
        chunkDataDictionary.Clear();
        foreach (ChunkRender chunk in chunkDictionary.Values)
        {
            Destroy(chunk.gameObject);
        }
        chunkDictionary.Clear();

        for (int x = 0; x < mapSizeInChunks; x++)
        {
            for (int z = 0; z < mapSizeInChunks; z++)
            {

                ChunkData data = new ChunkData(chunkSize, chunkHeight, this, new Vector3Int(x * chunkSize, 0, z * chunkSize));
                GenerateVoxels(data);
                chunkDataDictionary.Add(data.worldPosition, data);
            }
        }

        foreach (ChunkData data in chunkDataDictionary.Values)
        {
            MeshData meshData = Chunk.GetChunkMeshData(data);
            GameObject chunkObject = Instantiate(chunkPrefab, data.worldPosition, Quaternion.identity);
            ChunkRender chunkRender = chunkObject.GetComponent<ChunkRender>();
            chunkDictionary.Add(data.worldPosition, chunkRender);
            chunkRender.InitializeChunk(data);
            chunkRender.UpdateChunk(meshData);

        }
    }

    private void GenerateVoxels(ChunkData data)
    {
        for (int x = 0; x < data.chunkSize; x++)
        {
            for (int z = 0; z < data.chunkSize; z++)
            {
                float noiseValue = Mathf.PerlinNoise((data.worldPosition.x + x) * noiseScale, (data.worldPosition.z + z) * noiseScale);
                int groundPosition = Mathf.RoundToInt(noiseValue * chunkHeight);
                for (int y = 0; y < chunkHeight; y++)
                {
                    BlockType voxelType = BlockType.Dirt;
                    if (y > groundPosition)
                    {
                        if (y < waterThreshold)
                        {
                            voxelType = BlockType.Water;
                        }
                        else
                        {
                            voxelType = BlockType.Air;
                        }

                    }
                    else if (y == groundPosition)
                    {
                        voxelType = BlockType.Grass_Dirt;
                    }

                    Chunk.SetBlock(data, new Vector3Int(x, y, z), voxelType);
                }
            }
        }
    }

    internal BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        Vector3Int pos = Chunk.ChunkPositionFromBlockCoords(this, x, y, z);
        ChunkData containerChunk = null;

        chunkDataDictionary.TryGetValue(pos, out containerChunk);

        if (containerChunk == null)
            return BlockType.Nothing;
        Vector3Int blockInCHunkCoordinates = Chunk.GetBlockInChunkCoordinates(containerChunk, new Vector3Int(x, y, z));
        return Chunk.GetBlockFromChunkCoordinates(containerChunk, blockInCHunkCoordinates);
    }
}
