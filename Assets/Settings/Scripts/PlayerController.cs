using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem; // New Input System

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;

    public float moveSpeed = 2f;
    public float jumpForce = 3f;

    public float maxHealth = 100f;
    public float currentHealth;
    // tank barrel
    // public Transform tankBarrel;
    // private float aimAngle;
    // private float barrelRotationSpeed = 0.5f;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // guess this only detects for basic projectile...
        if (collision.gameObject.CompareTag("TankProjectile"))
        {
            // subtract from tank health
            currentHealth -= 25;
            Debug.Log("tank 1 current health");
            Debug.Log(currentHealth);
        }

    }

    void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("tank 1 movement detected...");
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {

        // Jump
        // if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        // {
        //     rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // }

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);


        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = targetVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}
