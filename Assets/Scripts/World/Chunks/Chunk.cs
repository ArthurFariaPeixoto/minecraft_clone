using System;
using System.Security.Cryptography;
using UnityEngine;

public static class Chunk
{
    public static void LoopThroughBlock(ChunkData chunkData, Action<int, int, int> action)
    {
        for (int index = 0; index < chunkData.blocks.Length; index++)
        {
            var position = GetPositionFromIndex(chunkData, index);
            action(position.x, position.y, position.z);
        }
    }

    private static Vector3Int GetPositionFromIndex(ChunkData chunkData, int index)
    {
        int x = index % chunkData.chunkSize;
        int y = (index / chunkData.chunkSize) % chunkData.chunkHeight;
        int z = index / (chunkData.chunkSize * chunkData.chunkHeight);

        return new Vector3Int(x, y, z);
    }
    private static int GetIndexFromPosition(ChunkData chunkData, int x, int y, int z)
    {
        return x + chunkData.chunkSize * y + chunkData.chunkSize * chunkData.chunkHeight * z;
    }

    // in chunk coordinate system
    private static bool InRange(ChunkData chunkData, int axisCoordinate)
    {
        if (axisCoordinate < 0 || axisCoordinate >= chunkData.chunkSize) return false;
        return true;
    }
    // in chunk coordinate system
    private static bool InRangeHeight(ChunkData chunkData, int yCoordinate)
    {
        if (yCoordinate < 0 || yCoordinate >= chunkData.chunkHeight) return false;
        return true;
    }

    public static void SetBlock(ChunkData chunkData, Vector3Int localPosition, BlockType block)
    {
        if (InRange(chunkData, localPosition.x) && InRangeHeight(chunkData, localPosition.y) && InRange(chunkData, localPosition.z))
        {
            int index = GetIndexFromPosition(chunkData, localPosition.x, localPosition.y, localPosition.z);
            chunkData.blocks[index] = block;
        }
        else
        {
            throw new Exception("Position out of range, need to ask World for appropiate chunk");
        }
    }

    public static Vector3Int GetBlockInChunkCoordinates(ChunkData chunkData, Vector3Int position)
    {
        return new Vector3Int
        {
            x = position.x - chunkData.worldPosition.x,
            y = position.y - chunkData.worldPosition.y,
            z = position.z - chunkData.worldPosition.z
        };
    }

    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, int x, int y, int z)
    {
        if (InRange(chunkData, x) && InRangeHeight(chunkData, y) && InRange(chunkData, z))
        {
            return chunkData.blocks[GetIndexFromPosition(chunkData, x, y, z)];
        }
        throw new Exception("Position out of range, need to ask World for appropiate chunk");
    }

    public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int chunckCoordinates)
    {
        return GetBlockFromChunkCoordinates(chunkData, chunckCoordinates.x, chunckCoordinates.y, chunckCoordinates.z);
    }

    public static MeshData GetChunkMeshData(ChunkData chunkData)
    {
        MeshData meshData = new(true);
        return meshData;
    }
}