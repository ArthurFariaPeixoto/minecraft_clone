using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int mapSizeInChunks = 17;
    public int chunkSize = 32, chunkHeight = 32, maxHeight = 256;
    public GameObject chunkPrefab;

    public TerrainGenerator terrainGenerator;
    public Vector2Int mapSeedOffset;

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
                // GenerateVoxels(data);
                ChunkData newData = terrainGenerator.GenerateChunkData(data, mapSeedOffset);
                chunkDataDictionary.Add(newData.worldPosition, newData);

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
