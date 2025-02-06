using UnityEngine;

public class GameConfig : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0; // Desativa V-Sync
        Application.targetFrameRate = 144; // Define FPS desejado
    }
}
