using UnityEngine;
using UnityEngine.InputSystem;
public class Tank02Barrel : MonoBehaviour
{

    public float rotationSpeed = 50f;
    private float verticalInput;
    public float minAngle = -90f;
    public float maxAngle = 90f;

    private float currentAngle = 0f;


    public GameObject muzzleFlashtPrefab;
    public GameObject muzzleSmokePrefab;

    // Projectile vars
    // Header to show within inspector for our script...
    [Header("Firing Settings")]
    public GameObject bulletPrefab; // Drag your bullet prefab here in Inspector
    public Transform firePoint;     // Drag your FirePoint object here in Inspector
    public float bulletForce = 20f;

    // use Update() for capturing input
    // use FixedUpdate() for physics based stuff...
    void Update()
    {
        if (Keyboard.current == null) return;

        RotateBarrel();
        // float rotationInput = 0;

        // // Using Left/Right or Up/Down arrows to rotate
        // if (Keyboard.current.wKey.isPressed) rotationInput = 1;
        // if (Keyboard.current.sKey.isPressed) rotationInput = -1;

        // // In 2D, we rotate around the Z axis
        // transform.Rotate(0, 0, rotationInput * rotationSpeed * Time.deltaTime);

        // float input = 0;

        // // Up arrow moves toward 180 (Left), Down arrow moves toward 0 (Right)
        // if (Keyboard.current.upArrowKey.isPressed) input = 1;
        // if (Keyboard.current.downArrowKey.isPressed) input = -1;

        
        // Shoot projectile with space bar key press
        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     Shoot();
        // }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Debug.Log("shoot for tank 2 pressed...");
        if (context.performed) {
            Shoot();
        }
    }

    public void OnBarrelRotate(InputAction.CallbackContext context)
    {
        Debug.Log("rotate barrel for tank 2 pressed...");
        Vector2 fullInput = context.ReadValue<Vector2>();

        // Grab only Y axis for move controls so just W and S
        verticalInput = fullInput.y;
    }

    private void RotateBarrel()
    {
        // 1. Calculate the new angle based on input and time
        currentAngle += verticalInput * rotationSpeed * Time.deltaTime;

        // 2. Clamp the angle so it stays between 0 and 180
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        // 3. Apply the rotation to the Z axis
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

    }


    // shoot script for projectile
    void Shoot()
    {
        // start muzzle animation
        GameObject muzzleEffect = Instantiate(muzzleFlashtPrefab, firePoint.position, firePoint.rotation);
        // start muzzle smoke
        GameObject muzzleSmokeEffect = Instantiate(muzzleSmokePrefab, firePoint.position, firePoint.rotation);

        // 1. Create the bullet at the FirePoint's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // make sure a tanks projectile doesn't explode on itself, on shoot
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        // 2. Get the Rigidbody2D to make it move
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // 3. Push the bullet in the direction the firePoint is facing (up for 2D sprites)
        // If your bullet flies sideways, change 'up' to 'right'
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        // destroy muzzle effect
        Destroy(muzzleEffect, 0.2f);
        Destroy(muzzleSmokeEffect, 6f);
    }

}
