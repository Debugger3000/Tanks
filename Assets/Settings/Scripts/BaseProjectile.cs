using UnityEngine;
using UnityEngine.Tilemaps;

// Base projectile class for all other projectile types to inherit from
public abstract class BaseProjectile : MonoBehaviour
{
    private float projectileDamage;
    public WeaponData weaponData;
    // protected AudioSource projectileAudioSource;
    // start function
    void Start()
    {
        //SetDamage(); // set damage attribute on projectile creation...
        // projectileAudioSource = GetComponent<AudioSource>();
    }

    void SetDamage()
    {
        projectileDamage = weaponData.damage;
    }

    public virtual float GetDamage()
    {
        return projectileDamage;
    }

    public void Setup(WeaponData weaponData)
    {
        Debug.Log($"Setting projectileDamage in BaseProjectile... to {weaponData}");
        this.weaponData = weaponData; // set data for projectile that just spawned
        SetDamage();
    }


    public virtual void Explode(Tilemap map, Vector2 impactPoint, float explosionRadius)
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
                
                // Delete tilemap from radius inwards
                if (Vector3.Distance(cellWorldPos, (Vector3)impactPoint) <= explosionRadius)
                {
                    map.SetTile(tilePos, null); // Delete tilemap
                }
            }
        }

        // enviuronment damage so we can play audio
        AudioManager.Instance.PlayEnvironmentHit(); // call audio manager to play...
    }
}
