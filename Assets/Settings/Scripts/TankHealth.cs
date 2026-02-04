using UnityEngine;

public class TankHealth : MonoBehaviour
{

    [SerializeField] private float maxTankHealth = 100f;
    [SerializeField] private float currentTankHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTankHealth = maxTankHealth;
        
    }

    // check for projectile collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the thing that hit us is a projectile
        if (collision.gameObject.CompareTag("TankProjectile"))
        {
            TankHit(10f); // Decrease health by 10
            
            // Optional: Destroy the projectile on impact
            //Destroy(collision.gameObject);
        }
    }

    // deal with tank damage
    private void TankHit(float damage)
    {
        // adjust actually tank health
        currentTankHealth = currentTankHealth - damage;

        // adjust canvas image for tank health
        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
