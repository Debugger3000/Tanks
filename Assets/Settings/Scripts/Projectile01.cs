using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile01 : BaseProjectile
{
    //public float explosionRadius = 1f; // How many tiles to destroy
    // public GameObject hitEffectPrefab;

    // private WeaponData data;

    void Start()
    {
    }

    // Call this to pass in weapon data to determine 
    // public void Setup(WeaponData weaponData)
    // {
    //     data = weaponData;
    // }

    public override void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded)
    {
        // do nothing
        // no special effects on this tank weapon...
    }





    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     ContactPoint2D contact = collision.GetContact(0);
    //     Vector2 hitPoint = contact.point;
        
    //     // Calculate rotation 
    //     Vector2 normal = collision.contacts[0].normal;
    //     float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
    //     Quaternion hitRotation = Quaternion.Euler(0, 0, angle);

        
    //     // Check if we hit the ground
    //     if (collision.gameObject.CompareTag("GroundDestruct"))
    //     {
    //         // spawn projectile explosion effect
    //         GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);    
        
    //         // Try to get the Tilemap component from what we hit
    //         Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();

    //         if (tilemap != null)
    //         {
    //             base.Explode(tilemap, collision.contacts[0].point, weaponData.explosionRadius);
    //             // Explode(tilemap, collision.contacts[0].point);
    //         }

    //         // Destroy the bullet itself
    //         Destroy(gameObject);
    //         Destroy(effect, 3f);
    //         // projectile has exploded switch turn now...
    //         GameController.Instance.SwitchTurn();
    //     }

    //     else if (collision.gameObject.layer == LayerMask.NameToLayer("Tanks")) 
    //     {
    //         // spawn projectile explosion effect
    //         GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);    
    //         // Destroy the bullet itself
    //         Destroy(gameObject);
    //         Destroy(effect, 3f);

    //         // call player hit function in player 

    //         // projectile has exploded switch turn now...
    //         GameController.Instance.SwitchTurn();
    //     }
        
        
    // }
}