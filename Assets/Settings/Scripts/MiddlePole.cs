using UnityEngine;
using UnityEngine.UI;

public class MiddlePole : MonoBehaviour
{

    // public so other scripts can edit this value... sometimes useful...
    //public float health = 100f;

    // make this private so only this script can edit pole health
    [Header("Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    [SerializeField] private GameObject healthBarCanvas;
    [SerializeField] private Image fillHealth;


    // set cur health and hide health bar when full... on start
    private void Start()
    {
        currentHealth = maxHealth;
        healthBarCanvas.SetActive(false);
    }


    // This is called automatically by Unity when a 2D collision occurs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the thing that hit us is a projectile
        if (collision.gameObject.CompareTag("TankProjectile"))
        {
            TakeDamage(10f); // Decrease health by 10
            
            // Optional: Destroy the projectile on impact
            //Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if(currentHealth < maxHealth)
        {
            healthBarCanvas.SetActive(true);
        }

        // update health bar visual after damage is subtracted
        fillHealth.fillAmount = currentHealth / 100; // fill is 0 - 1, so / by 10

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add particle effects or sound here before destroying
        Destroy(gameObject);
    }
}
