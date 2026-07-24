using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SideScrollPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool jumpRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // I-lock ang Z axis position at Rotation para manatiling 2.5D/3D Side-scroller
        rb.constraints = RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Kumuha ng A/D o Left/Right Arrow inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Ground Check Gamit ang OverlapSphere
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
        }

        // Jump Input Check
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequested = true;
        }

        // I-rotate ang character papunta sa nilalakaran
        RotateCharacter();
    }

    void FixedUpdate()
    {
        // 1. Lakad / Takbo (X-axis)
        rb.linearVelocity = new Vector3(horizontalInput * moveSpeed, rb.linearVelocity.y, 0f);

        // 2. Talon
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
            jumpRequested = false;
        }
    }

    private void RotateCharacter()
    {
        // Haharap sa kanan kapag D/Right Arrow, at sa kaliwa kapag A/Left Arrow
        if (horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    // Para makita mo ung Ground Check sphere sa Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}