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

    private Vector2 lastVelocity;

    private Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
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

            ExplosiveNearHit(); // check explosive radius for near hits

            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();

            isExploded = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Tanks") && !weaponData.isMineType) 
        {
            // spawn projectile explosion effect
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);    
            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            isExploded = true;

            if (!isExploded)
            {
                TriggerSpecialWeaponEffect(hitPoint,hitRotation, isExploded);
            }

            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();
            isExploded = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Crate") && !weaponData.isMineType) 
        {
            // spawn projectile explosion effect
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);

            AudioManager.Instance.PlayEnvironmentHit(); // call audio manager to play...

            Crate crate = collision.gameObject.GetComponent<Crate>();
            Destroy(crate);
            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);

            if (!isExploded)
            {
                TriggerSpecialWeaponEffect(hitPoint,hitRotation, isExploded);
            }

            ExplosiveNearHit(); // check explosive radius for near hits
            
            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();

            isExploded = true;
        }
        // projectile hits a wall... it should bounce off
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) 
        {
            Debug.Log($"Projectile hit the wall..............................................");
            ProjectileBounce(collision);
            
        }
        else
        {
            // projectile has exploded switch turn now...
            GameController.Instance.SwitchTurn();
        }
    }


    public virtual void Explode(Tilemap map, Vector2 impactPoint, float explosionRadius)
    {

        // Convert world impact position to Tilemap cell position
        Vector3Int centerCell = map.WorldToCell(impactPoint);

        // Loop through a grid around the impact point. Destroy those tiles 
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


    protected void ProjectileBounce(Collision2D collision)
    {
        var contact = collision.contacts[0].normal;
        Vector2 reflectDirection = Vector2.Reflect(lastVelocity.normalized,contact);

        rb.linearVelocity = reflectDirection.normalized * 30f;
    }

    // use to track projectiles velocity before contacts and such...
    void Update()
    {
        // Record the velocity every frame before physics handles collisions
        lastVelocity = rb.linearVelocity;
    }


    // close hits should damage vehicle....
    protected void ExplosiveNearHit()
    {
        float totalRadius = weaponData.explosionRadius + weaponData.explosiveBuffer;
    
        // get Tanks mask
        int tankLayerMask = LayerMask.GetMask("Tanks");
        // use overlap circle cause its easy to grab radius and tanks layer hit
        Collider2D[] hitTanks = Physics2D.OverlapCircleAll(transform.position, totalRadius, tankLayerMask);

        // grabs tanks within blast near hit radius
        foreach (Collider2D hit in hitTanks)
        {
            // Check if the object has a health component
            if (hit.TryGetComponent(out TankController tankController))
            {
                // Apply the damage
                tankController.TankTakesDamage(weaponData.explosiveDamage,"nearhit");
            }
        }
    }
}
