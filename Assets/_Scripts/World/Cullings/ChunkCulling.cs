using UnityEngine;

public class ChunkCulling : MonoBehaviour
{
    private Camera mainCamera;
    private Renderer chunkRenderer;
    private MeshCollider chunkCollider;
    public float renderDistance = 1f; // Distância máxima de renderização personalizada

    private readonly int updateInterval = 20; // Verifica o culling a cada 30 frames
    private int frameCount = 0; // Contador de frames

    private void Start()
    {
        mainCamera = Camera.main;
        chunkRenderer = GetComponent<Renderer>();
        chunkCollider = GetComponent<MeshCollider>();
    }

    private void LateUpdate() // Usa LateUpdate para menor impacto na renderização
    {
        if (mainCamera == null || chunkRenderer == null)
            return;

        // 🔹 Executa o culling apenas a cada 'updateInterval' frames
        if (frameCount % updateInterval != 0)
        {
            frameCount++;
            return;
        }
        frameCount++;

        Plane[] frustumPlanes = CullingManager.FrustumPlanes;
        Bounds chunkBounds = chunkRenderer.bounds;

        // Testa se o chunk está no frustum
        bool isVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, chunkBounds);

        // Se não está no frustum, mantém ativo se estiver dentro da distância personalizada
        if (!isVisible)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
            isVisible = distance < renderDistance;
        }

        // Em vez de SetActive(), desativamos apenas a renderização e colisão
        chunkRenderer.enabled = isVisible;
        if (chunkCollider != null)
            chunkCollider.enabled = isVisible;
    }
}