using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < 200; x++)
        {
            for (int z = 0; z < 200; z++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(x, 0, z);
                cube.layer = LayerMask.NameToLayer("Ground");

            }
        }
    }


}
