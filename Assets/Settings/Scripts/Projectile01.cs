using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile01 : MonoBehaviour
{
    public float explosionRadius = 1.5f; // How many tiles to destroy
    public GameObject hitEffectPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 hitPoint = contact.point;
        
        // Calculate rotation 
        Vector2 normal = collision.contacts[0].normal;
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        Quaternion hitRotation = Quaternion.Euler(0, 0, angle);

        // spawn projectile explosion effect
        GameObject effect = Instantiate(hitEffectPrefab, hitPoint, hitRotation);    
        
        // Check if we hit the ground
        if (collision.gameObject.CompareTag("GroundDestruct"))
        {
            // Try to get the Tilemap component from what we hit
            Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();

            if (tilemap != null)
            {
                Explode(tilemap, collision.contacts[0].point);
            }

            
        }
        // we hit a tank
        // else if(collision.gameObject.CompareTag("TankShellCollider"))
        // {
        //     ExplodeNoTiles(hitPoint);
        // }
        // Destroy the bullet itself
        Destroy(gameObject);
        
        Destroy(effect, 1.5f);
        // shell hits a tank, that shell needs to explode

    }

    


    void Explode(Tilemap map, Vector2 impactPoint)
    {
        // Convert world impact position to Tilemap cell position
        Vector3Int centerCell = map.WorldToCell(impactPoint);

        // Loop through a grid around the impact point
        // With 0.25 cells, a range of 5-6 will ensure a smooth circle
        // If your cell size is 0.125, you need a larger range to "find" all the tiny tiles
        int range = Mathf.CeilToInt(explosionRadius / 0.125f) + 1; 

        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int tilePos = new Vector3Int(centerCell.x + x, centerCell.y + y, 0);
                
                // Get the center of the tiny cell
                Vector3 cellWorldPos = map.GetCellCenterWorld(tilePos);
                
                // Check distance from the impact to this specific tiny cell
                if (Vector3.Distance(cellWorldPos, (Vector3)impactPoint) <= explosionRadius)
                {
                    map.SetTile(tilePos, null); // Deletes only this tiny 0.25 cell!
                }
            }
        }
    }
}