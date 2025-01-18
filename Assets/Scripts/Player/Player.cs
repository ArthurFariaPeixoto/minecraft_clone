using UnityEngine;

public class Player : MonoBehaviour
{
    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private Vector3 inputDirection;
    private Vector3 processedDirection;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.1f;
    private float playerHeight;
    private float raycastDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<BoxCollider>().size.y * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        // Obtém a direção de entrada do jogador (Horizontal e Vertical)
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (inputDirection != Vector3.zero)
        {
            // Converte a direção da entrada para o espaço da câmera
            processedDirection = ConvertToCameraSpace(inputDirection).normalized;
        }
        else
        {
            processedDirection = Vector3.zero;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (!isGrounded && groundCheckTimer <= 0f)
        {
            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, raycastDistance, groundLayer);
            groundCheckTimer = groundCheckDelay;
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Move o jogador no FixedUpdate para respeitar a física
        if (processedDirection != Vector3.zero)
        {
            MovePlayer(processedDirection);
        }

        ApplyJumpPhysics();
    }

    void MovePlayer(Vector3 direction)
    {
        rb.linearVelocity = new Vector3(direction.x * MoveSpeed, rb.linearVelocity.y, direction.z * MoveSpeed);

        if (isGrounded && direction == Vector3.zero)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    // Converte a direção de entrada para o espaço da câmera
    Vector3 ConvertToCameraSpace(Vector3 inputDirection)
    {
        // Obtém a referência para a câmera principal
        Transform cameraTransform = Camera.main.transform;

        // Calcula os vetores "forward" e "right" da câmera
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Remove a componente vertical para manter o movimento no plano horizontal
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normaliza os vetores para evitar mudanças de velocidade
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Converte a entrada para o espaço da câmera
        return cameraForward * inputDirection.z + cameraRight * inputDirection.x;
    }



    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Initial burst for the jump
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.linearVelocity += Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime * Vector3.up;
        }
        else if (rb.linearVelocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.linearVelocity += Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime * Vector3.up;
        }
    }

}