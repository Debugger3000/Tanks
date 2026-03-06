using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private string randomWeaponName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // assign weaponName to this crate on spawn
        randomWeaponName = GameController.Instance.GetRandomCrateWeapon();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Tank collided with a crate.....");
        if(collision.gameObject.layer == LayerMask.NameToLayer("Tanks"))
        {
            // get tank index for one hit
            TankController tankController = collision.gameObject.GetComponent<TankController>();
            int tankIndex = tankController.GetTankIndex();

            // give tank weapon
            GameController.Instance.TankHitsCrate(tankIndex,randomWeaponName);

            AudioManager.Instance.PlayHealCrateSFX(); 

            // destroy crate on tank contact 
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Projectiles"))
        {
            AudioManager.Instance.PlayEnvironmentHit();
            // destroy crate on tank contact 
            Destroy(gameObject);
        }
    }
}
