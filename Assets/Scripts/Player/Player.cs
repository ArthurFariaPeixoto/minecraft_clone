using UnityEngine;

public class Player : MonoBehaviour
{
    //GERAL
    private bool isFalling = false;

    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private Vector3 processedDirection;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private readonly float groundCheckDelay = 0.1f;
    private float playerHeight;
    private float raycastDistance;

    // Flying
    private bool isFlying = false;
    public float flightSpeed = 8f;
    public float flightAscendSpeed = 5f;
    private readonly float doubleTapTime = 0.25f;
    private float lastJumpTime = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<BoxCollider>().size.y * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        // // Hides the mouse
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
    void Update()
    {
        // Obtém a direção de entrada do jogador (Horizontal e Vertical)
        Vector3 inputDirection = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (inputDirection != Vector3.zero)
        {
            // Converte a direção da entrada para o espaço da câmera
            processedDirection = ConvertToCameraSpace(inputDirection).normalized;
        }
        else
        {
            processedDirection = Vector3.zero;
        }

        // Centraliza a lógica de salto, voo e verificação de solo
        HandleJumpAndFlight();
    }
    void FixedUpdate()
    {
        // Move the player in FixedUpdate to respect physics
        if (processedDirection != Vector3.zero)
        {
            MovePlayer(processedDirection);
        }

        if (isFlying)
        {
            ApplyFlightPhysics();
        }
        else
        {
            ApplyJumpPhysics();
        }
    }
    void MovePlayer(Vector3 direction)
    {
        float speed = isFlying ? flightSpeed : MoveSpeed;
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);

        if (isGrounded && direction == Vector3.zero)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
    Vector3 ConvertToCameraSpace(Vector3 inputDirection)
    {
        Transform cameraTransform = Camera.main.transform;

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * inputDirection.z + cameraRight * inputDirection.x;
    }
    void HandleJumpAndFlight()
    {
        // Verifica o estado de solo e queda
        if (!isFlying && !isGrounded && groundCheckTimer <= 0f)
        {
            isGrounded = IsOnGround();
            if (isGrounded) isFalling = false;
            groundCheckTimer = groundCheckDelay;
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

        // Lida com os controles de salto e voo
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && !isFlying) Jump();
            else if (CanStartFlying()) StartFlying();
            else if (CanStopFlying()) StopFlying();
            lastJumpTime = Time.time;
        }
    }
    bool CanStartFlying() => Time.time - lastJumpTime <= doubleTapTime && isFalling && !isFlying;
    bool CanStopFlying() => isFlying && !isGrounded && Time.time - lastJumpTime <= doubleTapTime;
    bool IsOnGround() => Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, raycastDistance, groundLayer);
    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        isFalling = true;
    }
    void StartFlying()
    {
        isFlying = true;
        rb.useGravity = false;
        isFalling = false;
    }
    void StopFlying()
    {
        isFlying = false;
        rb.useGravity = true;
        isFalling = true;
    }
    void ApplyFlightPhysics()
    {
        // Verifica entrada de voo vertical
        float ascend = Input.GetKey(KeyCode.Space) ? flightAscendSpeed : 0f;
        float descend = Input.GetKey(KeyCode.LeftControl) ? -flightAscendSpeed : 0f;

        // Se não houver entrada horizontal nem vertical, para completamente
        if (processedDirection == Vector3.zero && ascend == 0f && descend == 0f)
        {
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, ascend + descend, rb.linearVelocity.z);
        }

        // Sai do voo se estiver descendo e tocar o chão
        if (descend < 0f && IsOnGround())
        {
            StopFlying();
            isGrounded = true;
            isFalling = false;
        }
    }
    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime * Vector3.up;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime * Vector3.up;
        }
    }
}
