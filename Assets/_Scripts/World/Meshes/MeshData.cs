using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices = new();
    public List<int> triangles = new();
    public List<Vector2> uv = new();
    public List<Vector3> colliderVertices = new();
    public List<int> colliderTriangles = new();
    public MeshData waterMesh;
    private bool isMainMesh = true;

    public MeshData(bool isMainMesh)
    {
        if (isMainMesh)
        {
            waterMesh = new MeshData(false);
        }
    }

    public void AddVertex(Vector3 vertex, bool vertexGeneratesCollider)
    {
        vertices.Add(vertex);
        if (vertexGeneratesCollider)
        {
            colliderVertices.Add(vertex);
        }
    }

    public void AddQuadTriangles(bool quadGeneratesCollider)
    {
        triangles.Add(vertices.Count - 4); // 0
        triangles.Add(vertices.Count - 3); // 1
        triangles.Add(vertices.Count - 2); // 2

        triangles.Add(vertices.Count - 4); // 0
        triangles.Add(vertices.Count - 2); // 2
        triangles.Add(vertices.Count - 1); // 3

        if (quadGeneratesCollider)
        {
            colliderTriangles.Add(colliderVertices.Count - 4); // 0
            colliderTriangles.Add(colliderVertices.Count - 3); // 1
            colliderTriangles.Add(colliderVertices.Count - 2); // 2

            colliderTriangles.Add(colliderVertices.Count - 4); // 0
            colliderTriangles.Add(colliderVertices.Count - 2); // 2
            colliderTriangles.Add(colliderVertices.Count - 1); // 3
        }
    }
}