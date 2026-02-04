using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem; // New Input System

public class PlayerController : MonoBehaviour
{
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

    private Vector2 moveInput;

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

    void FixedUpdate()
    {

        // Jump
        // if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        // {
        //     rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // }

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);


        // Horizontal movement
        float move = 0f;
        if (Keyboard.current.aKey.isPressed) move = -1f;
        if (Keyboard.current.dKey.isPressed) move = 1f;

        Vector2 targetVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
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
