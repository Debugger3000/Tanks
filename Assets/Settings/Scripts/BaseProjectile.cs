using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

// Base projectile class for all other projectile types to inherit from
public abstract class BaseProjectile : MonoBehaviour
{
    private float projectileDamage;
    public WeaponData weaponData;

    protected bool isExploded = false;

    public bool isMineType = false;
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

    // Children implement this
    public abstract void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded);

    public void Setup(WeaponData weaponData)
    {
        Debug.Log($"Setting projectileDamage in BaseProjectile... to {weaponData}");
        this.weaponData = weaponData; // set data for projectile that just spawned
        SetDamage();
        if (weaponData.isMineType)
        {
            //isMineType = true; // set mine type to true
            OneTimeTriggerAfterProjectileFire(); // trigger this for mine type
            GameController.Instance.SwitchTurn(); // switch turn on first mine creation...
        }
    }

    public virtual void OneTimeTriggerAfterProjectileFire()
    {
        
        Debug.Log($"We set isExploded to true in baseproj: {isExploded}");
    }

    public virtual void SetIsExploded()
    {
        isExploded = true;
    }




    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 hitPoint = contact.point;
        
        // Calculate rotation 
        Vector2 normal = collision.contacts[0].normal;
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        Quaternion hitRotation = Quaternion.Euler(0, 0, angle);

        
        // Check if we hit the ground
        if (collision.gameObject.CompareTag("GroundDestruct") && !weaponData.isMineType)
        {
            // spawn projectile explosion effect
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);    
        
            // Try to get the Tilemap component from what we hit
            Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();

            if (tilemap != null)
            {
                Explode(tilemap, collision.contacts[0].point, weaponData.explosionRadius);
                // Explode(tilemap, collision.contacts[0].point);
            }

            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            if (!isExploded)
            {
                TriggerSpecialWeaponEffect(hitPoint,hitRotation, isExploded);
            }

            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Tanks") && !weaponData.isMineType) 
        {
            // spawn projectile explosion effect
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);    
            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            if (!isExploded)
            {
                TriggerSpecialWeaponEffect(hitPoint,hitRotation, isExploded);
            }

            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Crate") && !weaponData.isMineType) 
        {
            // spawn projectile explosion effect
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);

            Crate crate = collision.gameObject.GetComponent<Crate>();
            Destroy(crate);
            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            if (!isExploded)
            {
                TriggerSpecialWeaponEffect(hitPoint,hitRotation, isExploded);
            }


            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();
        }
        else
        {
            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();
            
        }
        isExploded = true;
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
