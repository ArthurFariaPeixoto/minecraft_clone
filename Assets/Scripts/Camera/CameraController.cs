using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Referência ao Transform do jogador
    public float mouseSensitivity = 2f; // Sensibilidade do mouse
    private float verticalRotation = 0f; // Rotação vertical acumulada
    private float horizontalRotation = 0f; // Rotação horizontal acumulada

    // Start is called before the first frame update
    void Start()
    {
        // Trava o cursor no centro da tela e o oculta
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        // Captura o movimento do mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Atualiza a rotação horizontal (esquerda/direita)
        horizontalRotation += mouseX;

        // Atualiza a rotação vertical (cima/baixo) e limita entre -90° e 90°
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        // Aplica a rotação vertical na câmera (apenas para cima/baixo)
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Aplica a rotação horizontal no player (esquerda/direita)
        player.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }


}