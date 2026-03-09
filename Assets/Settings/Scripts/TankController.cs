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
    private bool isJetpacking = false;
    public float jetpackForce = 45f; // Constant upward push
    public float gasDrainRateJetpack = 20f; // Fuel used per second
    public float maxJetpackHeight = 2f;

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
    [SerializeField]
    public float gasDrainRateJump = 20f;
   
    public AudioSource tankControllerAudioSource;
    public AudioSource tankIdleAudioSource;
    public AudioSource tankJetpackAudioSource;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public Vector2 boxSize = new Vector2(10.0f, 0.5f); // Width and Height
    public LayerMask groundLayer;

    private Vector2 moveInput; // horizontal movement var

    public GameObject jetpackParticles;
    private GameObject currentJetpackEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // set health to full
        currentGas = maxGas; // set gas to full

        var pInput = GetComponent<PlayerInput>();
        Debug.Log($"{gameObject.name} is Player Index: {pInput.playerIndex}");
        tankIndex = pInput.playerIndex;
    }

    // expose tankIndex to other functions
    public int GetTankIndex()
    {
        return tankIndex;
    }

    // On projectile hit
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // guess this only detects for basic projectile...
        if (collision.gameObject.TryGetComponent(out BaseProjectile projectile))
        {
            float damageOfProjectile = projectile.GetDamage();  
            TankTakesDamage(damageOfProjectile,"direct"); // subtract health from tank
        }
    }

    public void TankTakesDamage(float damageOfProjectile, string type)
    {
        currentHealth -= damageOfProjectile; // subtract from tank health
        if(currentHealth <= 0)
            {
                AudioManager.Instance.PlayTankHit(); // play tank hit audio...
                StartCoroutine(ActivateDelay());
                AudioManager.Instance.PlayTargetNeutralizedAnnouncer(); // play target hit announcer audio
                // Tank has zero health or less, end game
                GameController.Instance.OnPlayerDeath(tankIndex);
            }
        // direct hit
        else if(type == "direct")
            {
                AudioManager.Instance.PlayTankHit(); // play tank hit audio...
                StartCoroutine(ActivateDelay());
                GameController.Instance.TankDamage(tankIndex, currentHealth); // update UI health bar
            }
        else if(type == "nearhit")
            {
                StartCoroutine(ActivateDelay());
                AudioManager.Instance.PlayArtilleryInbound(); // play tank hit audio...
                // play artillery inbound
                GameController.Instance.TankDamage(tankIndex, currentHealth); // update UI health bar
            }
    }

    

    // delayed call, so animations can play out 
    IEnumerator ActivateDelay()
    {
        yield return new WaitForSeconds(2.0f);
    }

    void Update()
    {
        // this is needed so tanks don't slide, since gravity exists in the game
        // raycast below tank to know when grounded...
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

        if (isJetpacking && currentGas > 0 && isMyTurn)
        {
            // Raycast down to find the ground
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                float currentAltitude = hit.distance;

                // 2. Only apply force if we are below the limit
                if (currentAltitude < maxJetpackHeight)
                {
                    rb.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
                }
                else
                {
                    if(rb.linearVelocity.y > 0)
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                    }
                }
            }
            currentGas -= gasDrainRateJetpack * Time.fixedDeltaTime;
            GameController.Instance.TankGas(tankIndex, currentGas);
        }
        else
        {
            // Stop jetpacking if fuel runs out
            isJetpacking = false; 
        }

        // Horizontal movement
        // Check if there is horizontal input AND we have gas
        if (isMyTurn && Mathf.Abs(moveInput.x) > 0.01f && currentGas > 0f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            currentGas -= gasDrainRate * Time.deltaTime;

            Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = targetVelocity;

            // Update the UI/Controller
            GameController.Instance.TankGas(tankIndex, currentGas);

            if (!tankControllerAudioSource.isPlaying && !isJetpacking && isGrounded)
            {
                    tankControllerAudioSource.clip = AudioManager.Instance.tankEngineMoving;
                    tankControllerAudioSource.loop = true;
                    tankControllerAudioSource.Play();
            }
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

    // jetpack input
    public void OnJump(InputAction.CallbackContext context)
    {

        if (!isMyTurn) return;
        // make sure tank controller knows when player is pressing jetpack
        if (context.started && currentGas > 0) 
        {
            isJetpacking = true;
            if (tankControllerAudioSource.isPlaying)
            {
                tankControllerAudioSource.Stop(); // stop moving audio while jetpack is going...
            }
            // play audio
            tankJetpackAudioSource.clip = AudioManager.Instance.jetpack;
            tankJetpackAudioSource.loop = true;
            tankJetpackAudioSource.Play();

            Quaternion flipRotation = groundCheck.rotation * Quaternion.Euler(180, 0, 0);
            // Change your instantiation line to this:
            currentJetpackEffect = Instantiate(jetpackParticles, groundCheck.position, flipRotation, groundCheck);
        }
        else if (context.canceled) 
        {
            tankJetpackAudioSource.Stop();
            Destroy(currentJetpackEffect, 0.1f);
            isJetpacking = false;
        }
    }

    // horizontal input for movement
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isMyTurn)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    public void SetIsTurn(bool val)
    {   
        isMyTurn = val;
    }

    public void ResetGas()
    {
        currentGas = maxGas;
        GameController.Instance.TankGas(tankIndex, 100f);
    }

    public void GiveHealth()
    {
        float newHealth = currentHealth + 25;
        if(newHealth > 100f)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = newHealth;
        }
        GameController.Instance.TankDamage(tankIndex, currentHealth); // update UI
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }

    // Scroll wheel input for map zoom out/in
    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();
        if (scrollValue.y != 0)
        {
            GameController.Instance.CameraScroll(scrollValue);
        }
    }
}
