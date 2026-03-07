using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class TankBarrel : MonoBehaviour
{

    [SerializeField]
    private int tankIndex;
    public float rotationSpeed = 50f;
    [SerializeField]
    private float verticalInput;

    public float minAngle = -90f;
    public float maxAngle = 90f;

    [SerializeField]
    private float currentAngle = 0f;


    public GameObject muzzleFlashtPrefab;
    public GameObject muzzleSmokePrefab;

    private bool hasPlayerShot = false;

    private TankController myTankController;

    public AudioSource tankBarrelAudioSource;
    public AudioSource tankShootAudioSource;

    // Projectile vars
    // Header to show within inspector for our script...
    [Header("Firing Settings")]
    // public GameObject bulletPrefab;


    // current weapon / projectile
    // updated by UI from above... we swap to whatever weapon we want
    private WeaponInstance currentWeapon;

    // set current projectile for tank
    public void SetWeapon(WeaponInstance weapon)
    {
        currentWeapon = weapon; // change weapon data in barrel
        //projectileScript.Setup(currentWeapon); 
    }



    public Transform firePoint;     // Drag your FirePoint object here in Inspector
    // 25f is 100% power
    // 20f is 75% power
    // 15f is 50%
    // 10f
    // 5f
    public float bulletForce = 20f;

    void Awake() 
    {


        var pInput = GetComponentInParent<PlayerInput>();
        if (pInput != null)
        {
            tankIndex = pInput.playerIndex;                                 
            //Debug.Log($"Tank spawned! I am player: {tankIndex}");
        }
        // Looks up the hierarchy until it finds the TankController
        // myTankController = GetComponentInParent<TankController>();
        
        // if (myTankController == null) {
        //     Debug.LogError($"Barrel on {gameObject.name} can't find its TankController parent!");
        // }

        var tankControl = GetComponentInParent<TankController>();
        myTankController = tankControl; // set tankcontroller so we can grab vars

        tankBarrelAudioSource = GetComponent<AudioSource>(); // set audio source for barrel...
    }

    // void Start()
    // {
        
    // }

    public void SetHasPlayerShot(bool val)
    {
        hasPlayerShot = val;
    }

    // use Update() for capturing input
    // use FixedUpdate() for physics based stuff...
    void Update()
    {
        //if (!myTankController.isMyTurn) return;

        // call rotate barrel function
        RotateBarrel();

        // float rotationInput = 0;

        // // Using Left/Right or Up/Down arrows to rotate
        // if (Keyboard.current.wKey.isPressed) rotationInput = 1;
        // if (Keyboard.current.sKey.isPressed) rotationInput = -1;

        // // In 2D, we rotate around the Z axis
        // transform.Rotate(0, 0, rotationInput * rotationSpeed * Time.deltaTime);

        // float input = 0;

        // // Up arrow moves toward 180 (Left), Down arrow moves toward 0 (Right)
        // if (Keyboard.current.wKey.isPressed) input = 1;
        // if (Keyboard.current.sKey.isPressed) input = -1;

        
        // Shoot projectile with space bar key press
        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     Shoot();
        // }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        //Debug.Log("shoot for tank 1 pressed...");
        if (context.performed) {
            Shoot();
        }
    }

    public void OnBarrelRotate(InputAction.CallbackContext context)
    {
        // make sure only current tank barrel is rotating.
        if (myTankController.isMyTurn)
        {
            //Debug.Log($"rotate barrel for tank {tankIndex} pressed...");
            Vector2 fullInput = context.ReadValue<Vector2>();

            // Grab only Y axis for move controls so just W and S
            verticalInput = fullInput.y * -1; // flip direction

    
            // If the input is active (not zero)
            if (verticalInput != 0)
            {
                // Only start playing if it's not already playing (prevents stutter)
                if (!tankBarrelAudioSource.isPlaying)
                {
                    tankBarrelAudioSource.clip = AudioManager.Instance.tankBarrel;
                    tankBarrelAudioSource.loop = true;
                    tankBarrelAudioSource.Play();
                }
            }
            else
            {
                // User let go of the button or stick is neutral
                tankBarrelAudioSource.Stop();
            }


        }
       
    }

    private void RotateBarrel()
    {
        // 1. Calculate the new angle based on input and time
        currentAngle += verticalInput * rotationSpeed * Time.deltaTime;

        // 2. Clamp the angle so it stays between 0 and 180
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        //Debug.Log($"tank {tankIndex} moveInput {currentAngle}");

        // 3. Apply the rotation to the Z axis
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
        
    }

    // private float GetPowerPercent()
    // {
    //     return (bulletForce - 20f) / 20f;
    // }

    // 35f is 100% power
    // 30f is 75% power
    // 25f is 50%
    // 20f is 25%

    public void OnIncreasePower(InputAction.CallbackContext context)
    {
        //Debug.Log($"{gameObject.name} moved by {context.control.name}");

        if (context.performed) {

            // play audio
            tankBarrelAudioSource.PlayOneShot(AudioManager.Instance.powerChangeClick);

            if(bulletForce < 35f)
            {
                bulletForce += 1;
            }
            
            // float powerPercent = 0.25f + (bulletForce - 10f) * (0.75f / 15f);
            float powerPercent = 0.25f + (bulletForce - 20f) * (0.75f / 15f);
            GameController.Instance.SetPowerBar(tankIndex, powerPercent);
        }
        
    }
    public void OnDecreasePower(InputAction.CallbackContext context)
    {
        //Debug.Log($"{gameObject.name} moved by {context.control.name}");
        if (context.performed) {

            // play audio
            tankBarrelAudioSource.PlayOneShot(AudioManager.Instance.powerChangeClick);

            if(bulletForce > 20f)
            {
                bulletForce -= 1;
            }
            // float powerPercent = 0.25f + (bulletForce - 10f) * (0.75f / 15f);
            float powerPercent = 0.25f + (bulletForce - 20f) * (0.75f / 15f);
            GameController.Instance.SetPowerBar(tankIndex, powerPercent);
        }
    }


    // shoot script for projectile
    void Shoot()
    {
        // make sure player can only shoot once per turn
        if (!hasPlayerShot && currentWeapon.currentAmmo > 0)
        {
            // if player has shot
            hasPlayerShot = true;
           
            InitShoot(); // start shot logic...        
        }
        // current weapon is out of ammo...
        else if (!hasPlayerShot && currentWeapon.currentAmmo < 1)
        {
            // play audio
            tankBarrelAudioSource.PlayOneShot(AudioManager.Instance.powerChangeClick);
        }
    }


    public void InitShoot()
    {   
        // stop this tanks idle noise, now that there turn is over
        myTankController.tankIdleAudioSource.Stop();
        // play this, wait a second then fire shot...
        AudioManager.Instance.PlayFireAtWillAnnouncer(); // play fire announcer audio
        StartCoroutine(SwitchTurnDelayed());
    
        //Invoke("SwitchTurnDelayed", turnDelay); // 
    }

    // delayed call, so animations can play out 
    IEnumerator SwitchTurnDelayed()
    {
        
        yield return new WaitForSeconds(2.0f);

        // play audio
        tankShootAudioSource.PlayOneShot(AudioManager.Instance.tankFire);

            // start muzzle animation
            GameObject muzzleEffect = Instantiate(muzzleFlashtPrefab, firePoint.position, firePoint.rotation);
            // start muzzle smoke
            GameObject muzzleSmokeEffect = Instantiate(muzzleSmokePrefab, firePoint.position, firePoint.rotation);

            // create projectile at muzzle point
            GameObject bullet = Instantiate(currentWeapon.weaponData.projectilePreFab, firePoint.position, firePoint.rotation);

            // have camera follow projectile
            GameController.Instance.ProjectileShotCameraView(bullet.transform); // set camera to wide view...

            // The Handoff: The Barrel gives the Projectile a reference to the data
            if (bullet.TryGetComponent(out BaseProjectile baseProjectileScript))
            {
                baseProjectileScript.Setup(currentWeapon.weaponData);

            }

            // make sure a tanks projectile doesn't explode on itself, on shoot
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // get rb for projectile
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // propel the projectile
            // rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            // Instead of AddForce
            rb.linearVelocity = firePoint.up * bulletForce;

            // destroy muzzle effect
            Destroy(muzzleEffect, 0.3f);
            Destroy(muzzleSmokeEffect, 6f);

            

            // decrement weapon ammo by 1 after use
            //currentWeapon.currentAmmo -= 1;

            // update UI for weapon icon
            GameController.Instance.WeaponAmmoDecrement(tankIndex, currentWeapon.weaponData.weaponName); // decrement by 1
            }


    
  
}
