using UnityEngine;
using UnityEngine.UI;

public class MiddlePole : MonoBehaviour
{
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

    // collision logic with projectiles...
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the thing that hit us is a projectile
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            TakeDamage(10f); // Decrease health by 10
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
