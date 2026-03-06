using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 20f;
    public float maxSpeed = 6f;

    [Header("Jump")]
    public float jumpImpulse = 8f;
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundMask;

    public Transform finish;

    Rigidbody2D rb;
    bool jumpRequested;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        bool isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundMask
        );

        float move = 0f;
        if (Keyboard.current.leftArrowKey.isPressed)
            move = -1f;
        else if (Keyboard.current.rightArrowKey.isPressed)
            move = 1f;

        rb.AddForce(Vector2.right * move * moveForce);

        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(
                Mathf.Sign(rb.linearVelocity.x) * maxSpeed,
                rb.linearVelocity.y
            );
        }

        if (jumpRequested && isGrounded)
        {
            // Reset vertical velocity before jumping
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
        }

        jumpRequested = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}