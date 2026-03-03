using UnityEngine;
using UnityEngine.Tilemaps;

public class Mine : BaseProjectile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded)
    {
        // do nothing
        // no special effects on this tank weapon...

        //GameObject bullet = Instantiate(clusterPrefab, hitPoint, hitRotation);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Run the BaseProjectile logic first
        // base.OnCollisionEnter2D(collision);    

        ContactPoint2D contact = collision.GetContact(0);
        Vector2 hitPoint = contact.point;
        
        // Calculate rotation 
        Vector2 normal = collision.contacts[0].normal;
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        Quaternion hitRotation = Quaternion.Euler(0, 0, angle);

        
        // Check if we hit the ground
        if (collision.gameObject.CompareTag("GroundDestruct"))
        {
            Debug.Log("MINe hit ground NEXT TURNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
            // spawn projectile explosion effect
            // GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);    
        
            // // Try to get the Tilemap component from what we hit
            // Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();

            // if (tilemap != null)
            // {
            //     Explode(tilemap, collision.contacts[0].point, weaponData.explosionRadius);
            //     // Explode(tilemap, collision.contacts[0].point);
            // }

            // Destroy the bullet itself
            // Destroy(gameObject);
            // Destroy(effect, 3f);

            // if (!isExploded)
            // {
            //     TriggerSpecialWeaponEffect(hitPoint,hitRotation, isExploded);
            // }

            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Tanks"))
        {
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);  

            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            GameController.Instance.SwitchTurn();
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Crate"))
        {
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);

            Crate crate = collision.gameObject.GetComponent<Crate>();
            Destroy(crate);

            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);



            // GameController.Instance.SwitchTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
