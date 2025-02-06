using UnityEngine;

public class CullingManager : MonoBehaviour
{
    public static Plane[] FrustumPlanes { get; private set; }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            // 🔹 Obtém os planos do frustum
            FrustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

            // 🔹 Expande os planos manualmente para aumentar a área visível
            for (int i = 0; i < FrustumPlanes.Length; i++)
            {
                FrustumPlanes[i].distance += 1f; // Aumenta a área de visão em 8 unidades
            }
        }
    }
}