using UnityEngine;
using UnityEngine.Tilemaps;

public class ClusterBomb : BaseProjectile
{
    //public float explosionRadius = 1f; // How many tiles to destroy
    // public GameObject hitEffectPrefab;

    // public WeaponData data;

    public float clusterForce = 5f;

    public GameObject clusterPrefab;

    private bool isExploded = false;

    // Call this to pass in weapon data to determine 
    // public void Setup(WeaponData weaponData)
    // {
    //     data = weaponData;
    // }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 hitPoint = contact.point;
        
        // Calculate rotation 
        Vector2 normal = collision.contacts[0].normal;
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        Quaternion hitRotation = Quaternion.Euler(0, 0, angle);

        
        // Check if we hit the ground
        if (collision.gameObject.CompareTag("GroundDestruct"))
        {
            // spawn projectile explosion effect
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);    
        

            // Try to get the Tilemap component from what we hit
            Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();

            if (tilemap != null)
            {
                base.Explode(tilemap, collision.contacts[0].point, weaponData.explosionRadius);
                // Explode(tilemap, collision.contacts[0].point);
            }

            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();

            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            // call cluster bombs out
            if (!isExploded)
            {
                CreateClusters(hitPoint,hitRotation);
            }

            
        }
        // we hit a wall so it should bounce...
        // else if(collision.gameObject.CompareTag("TankShellCollider"))
        // {
            
        // }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Tanks")) 
        {
            // spawn projectile explosion effect
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation); 

            
            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();

            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            // call cluster bombs out
            if (!isExploded)
            {
                CreateClusters(hitPoint,hitRotation);
            }
            
        }
    }

    
    // function to call to release the 5 mini bombs
    private void CreateClusters(Vector2 hitPoint, Quaternion hitRotation)
    {
        if (!isExploded)
        {
            for (int i = 0; i < 5; i++)
        {

            Debug.Log($"Mini cluster : {i} created....");
            GameObject bullet = Instantiate(clusterPrefab, hitPoint, hitRotation);

            // The Handoff: The Barrel gives the Projectile a reference to the data
            if (bullet.TryGetComponent(out BaseProjectile baseProjectileScript))
            {
                baseProjectileScript.Setup(weaponData.childWeaponData[0]); 
            }

            float randomAngle = Random.Range(250f, 310f);

            // 3. Apply that rotation to the Z-axis (which is 'forward' in 2D)
            // This adds the random angle to the existing hitRotation
            bullet.transform.Rotate(0, 0, randomAngle);

            // make sure a tanks projectile doesn't explode on itself, on shoot
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // make sure mini bombs don't instant explode on direct tank hits...
            

            // 2. Get the Rigidbody2D to make it move
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // Push cluster mini out to a random direction
            rb.AddForce(bullet.transform.up * clusterForce, ForceMode2D.Impulse);
        }
           isExploded = true; // make sure we only trigger mini clusters once... 
        }
        
    }


    // void Explode(Tilemap map, Vector2 impactPoint)
    // {
    //     // Convert world impact position to Tilemap cell position
    //     Vector3Int centerCell = map.WorldToCell(impactPoint);

    //     // Loop through a grid around the impact point
    //     // With 0.25 cells, a range of 5-6 will ensure a smooth circle
    //     // If your cell size is 0.125, you need a larger range to "find" all the tiny tiles
    //     int range = Mathf.CeilToInt(data.explosionRadius / 0.125f) + 1; 

    //     for (int x = -range; x <= range; x++)
    //     {
    //         for (int y = -range; y <= range; y++)
    //         {
    //             Vector3Int tilePos = new Vector3Int(centerCell.x + x, centerCell.y + y, 0);
                
    //             // Get the center of the tiny cell
    //             Vector3 cellWorldPos = map.GetCellCenterWorld(tilePos);
                
    //             // Delete tilemap from radius inwards
    //             if (Vector3.Distance(cellWorldPos, (Vector3)impactPoint) <= data.explosionRadius)
    //             {
    //                 map.SetTile(tilePos, null); // Delete tilemap
    //             }
    //         }
    //     }
    // }
}
