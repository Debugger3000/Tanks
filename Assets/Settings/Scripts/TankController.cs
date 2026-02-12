using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class TankController : MonoBehaviour
{

    [SerializeField]
    private int tankIndex;
    public bool isMyTurn = false;

    public float moveSpeed = 2f;
    public float jumpForce = 3f;

    [SerializeField]
    public int shellPower = 75;

    [SerializeField]
    public float maxHealth = 100f;
    [SerializeField]
    public float currentHealth;

    [SerializeField]
    public float maxGas = 100f;
    public float currentGas;
    [SerializeField]
    public float gasDrainRate = 10f;



    // tank barrel
    

    private Rigidbody2D rb;
    private bool isGrounded = false;

    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask groundLayer;

    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // set health to full
        currentGas = maxGas; // set gas to full

        var pInput = GetComponent<PlayerInput>();
        Debug.Log($"{gameObject.name} is Player Index: {pInput.playerIndex}");
        tankIndex = pInput.playerIndex;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // guess this only detects for basic projectile...
        if (collision.gameObject.CompareTag("TankProjectile"))
        {
            // subtract from tank health
            currentHealth -= 25;
            //Debug.Log("tank 1 current health");
            //Debug.Log(currentHealth);
            GameController.Instance.TankDamage(tankIndex, currentHealth);
        }

    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // if not players turn, dont do anything with input
        if (!isMyTurn) return;

        //Debug.Log($"current gas: {currentGas}");
        // Jump
        // if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        // {
        //     rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // }

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Horizontal movement
        // float move = 0f;
        // if (Keyboard.current.leftArrowKey.isPressed) move = -1f;
        // if (Keyboard.current.rightArrowKey.isPressed) move = 1f;
        // Check if there is horizontal input AND we have gas
        if (isMyTurn && Mathf.Abs(moveInput.x) > 0.01f && currentGas > 0f)
        {
            //Debug.Log($"tank {tankIndex} moveInput {moveInput.x}");

            // Drain gas based on TIME, not per key-press
            // '5 * Time.deltaTime' drains 5 units per second
            currentGas -= gasDrainRate * Time.deltaTime;

            Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = targetVelocity;

            // Update the UI/Controller
            GameController.Instance.TankGas(tankIndex, currentGas);
        }
            
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // if not players turn, dont do anything with input
        //if (!isMyTurn) return;
        Debug.Log($"tank {tankIndex} OnMove - IsmyTurn: {isMyTurn}");
        //Debug.Log($"{gameObject.name} moved by {context.control.name} " + $"using scheme: {GetComponent<PlayerInput>().currentControlScheme}");
        moveInput = context.ReadValue<Vector2>();
        // if (Mathf.Abs(moveInput.x) > 0.01f)
        // {
        //         if(currentGas > 0f)
        //         {
        //             // drain gas with each movement update
        //             currentGas -= 5;
        //             GameController.Instance.TankGas(tankIndex,currentGas);
        //         }
            
        // }
    }

    public void SetIsTurn(bool val)
    {   
        
        isMyTurn = val;
        Debug.Log($"is turn flag to: {isMyTurn} for TANK{tankIndex}");
    }

    public void ResetGas()
    {
        Debug.Log($"Gas has been filled {tankIndex}");
        currentGas = maxGas;
        GameController.Instance.TankGas(tankIndex, 100f);

        Debug.Log(currentGas);
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
