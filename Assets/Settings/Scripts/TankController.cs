using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class TankController : MonoBehaviour
{

    [SerializeField]
    private int tankIndex;
    public bool isMyTurn = false;

    public float moveSpeed = 2f;
    public float jumpForce = 3f;

    public float tiltSpeed = 5f;

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

    public AudioSource tankControllerAudioSource;
    public AudioSource tankIdleAudioSource;



    // tank barrel
    

    private Rigidbody2D rb;
    private bool isGrounded = false;

    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public Vector2 boxSize = new Vector2(10.0f, 2f); // Width and Height
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

        // set audio source
        // tankControllerAudioSource = GetComponent<AudioSource>();
    }

    // On projectile hit
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // guess this only detects for basic projectile...
        if (collision.gameObject.TryGetComponent(out BaseProjectile projectile))
        {
            float damageOfProjectile = projectile.GetDamage();  
            Debug.Log($"damage of projectile: {damageOfProjectile}");
            Debug.Log($"BEFORE currentHealth for tank: {currentHealth}");
            currentHealth -= damageOfProjectile; // subtract from tank health
            Debug.Log($"AFTER currentHealth for tank: {currentHealth}");
            GameController.Instance.TankDamage(tankIndex, currentHealth); // update UI health bar
            StartTankHitAudio(); // start tank hit audio...
        }
        /// else if(collision.gameObject.layer == LayerMask.NameToLayer("Crate"))
        // {
        // }
    }

    public void StartTankHitAudio()
    {   
        AudioManager.Instance.PlayTankHit(); // play tank hit audio...
        StartCoroutine(ActivateDelay());
    
        //Invoke("SwitchTurnDelayed", turnDelay); // 
    }

    // delayed call, so animations can play out 
    IEnumerator ActivateDelay()
    {
        yield return new WaitForSeconds(2.0f);
        AudioManager.Instance.PlayTargetHitAnnouncer(); // play target hit announcer audio

    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, boxSize, 0f, Vector2.down, checkRadius, groundLayer);

        if (hit.collider != null) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
        
    }

    void FixedUpdate()
    {
        // if not players turn, dont do anything with input
        if (!isMyTurn) return;

        // Raycast from front and back wheels
        // RaycastHit2D hitFront = Physics2D.Raycast(transform.position + transform.right * 0.5f, -transform.up, 2f, groundLayer);
        // RaycastHit2D hitBack = Physics2D.Raycast(transform.position - transform.right * 0.5f, -transform.up, 2f, groundLayer);

        // if (hitFront.collider != null && hitBack.collider != null) {
        //     // Find the direction from the back hit point to the front hit point
        //     Vector2 direction = hitFront.point - hitBack.point;
        //     // Calculate the target rotation
        //     float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
        //     // Smoothly rotate the tank
        //     Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        //     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        // }

        //Debug.Log($"current gas: {currentGas}");
        // Jump
        // if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        // {
        //     rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        // }

        // Check if grounded
        // isGrounded = Physics2D.OverlapCircle(groundCheck.position, boxSize, checkRadius, groundLayer);

        

        // Horizontal movement
        // float move = 0f;
        // if (Keyboard.current.leftArrowKey.isPressed) move = -1f;
        // if (Keyboard.current.rightArrowKey.isPressed) move = 1f;
        // Check if there is horizontal input AND we have gas
        if (isMyTurn && Mathf.Abs(moveInput.x) > 0.01f && currentGas > 0f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //Debug.Log($"tank {tankIndex} moveInput {moveInput.x}");

            // Drain gas based on TIME, not per key-press
            // '5 * Time.deltaTime' drains 5 units per second
            currentGas -= gasDrainRate * Time.deltaTime;

            Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = targetVelocity;

            // Update the UI/Controller
            GameController.Instance.TankGas(tankIndex, currentGas);

            if (!tankControllerAudioSource.isPlaying)
            {
                    tankControllerAudioSource.clip = AudioManager.Instance.tankEngineMoving;
                    tankControllerAudioSource.loop = true;
                    tankControllerAudioSource.Play();
            }
            // else
            // {
            //     // User let go of the button or stick is neutral
            //     tankControllerAudioSource.Stop();
            // }

        }
        //  no movement, same turn and gas is filled
        // we play idle sound
        else if(isMyTurn && Mathf.Abs(moveInput.x) < 0.01f && currentGas > 0f)
        {
            // no movement... so we need to stop engine moving audio
            tankControllerAudioSource.Stop();
            // Engine Idle Audio
            if (!tankIdleAudioSource.isPlaying)
            {
                    tankIdleAudioSource.clip = AudioManager.Instance.tankEngineIdle;
                    tankIdleAudioSource.loop = true;
                    tankIdleAudioSource.Play();
            }
            
        }
        // is turn and gas is empty... engine sounds need to stop...
        else if(isMyTurn && currentGas <= 0f)
        {
            // stop all engine sounds.... no gas...
            tankIdleAudioSource.Stop(); // stop idle
            tankControllerAudioSource.Stop(); // stop moving
        }
        
        // this should always happen for both tnaks whenever...
        // Tanks when touching environment / ground, are NOT affected by gravity...
        if (Mathf.Abs(moveInput.x) < 0.01f && isGrounded)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // if not players turn, dont do anything with input
        //if (!isMyTurn) return;
        Debug.Log($"tank {tankIndex} OnMove - IsmyTurn: {isMyTurn}");
        //Debug.Log($"{gameObject.name} moved by {context.control.name} " + $"using scheme: {GetComponent<PlayerInput>().currentControlScheme}");
        moveInput = context.ReadValue<Vector2>();

        // input detected...
        if (moveInput.x != 0)
        {
            if (!tankControllerAudioSource.isPlaying)
            {
                    tankControllerAudioSource.clip = AudioManager.Instance.tankEngineIdle;
                    tankControllerAudioSource.loop = true;
                    tankControllerAudioSource.Play();
            }
            
        }
        
        else
        {
            // User let go of the button or stick is neutral
            tankControllerAudioSource.Stop();
        }
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

    // private void OnDrawGizmosSelected()
    // {
    //     if (groundCheck != null)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    //     }
    // }
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(groundCheck.position, boxSize);
    // }
}
