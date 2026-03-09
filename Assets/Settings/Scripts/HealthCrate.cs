using UnityEngine;

public class HealthCrate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Tank collided with a health crate.....");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Tanks"))
        {

            TankController tankController = collision.gameObject.GetComponent<TankController>();
            int tankIndex = tankController.GetTankIndex();
            // give tank weapon
            GameController.Instance.TankHitsHealthCrate(tankIndex);
            AudioManager.Instance.PlayHealCrateSFX(); // play crate grabbed sound

            // destroy crate on tank contact 
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            // AudioManager.Instance.PlayEnvironmentHit();
            // destroy crate on tank contact 
            Destroy(gameObject);
        }
    }
}
