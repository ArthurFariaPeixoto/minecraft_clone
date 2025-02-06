using UnityEngine;

public class GameSettings : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0; // Desativa V-Sync
        Application.targetFrameRate = -1; // Define FPS desejado
    }
}
