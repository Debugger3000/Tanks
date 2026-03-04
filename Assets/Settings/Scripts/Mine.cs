using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mine : BaseProjectile
{
    protected int minesToSplitAmount = 2; // mines to break off

    public float mineSplitDelayTime = 2.5f;
    public float mineSeparationDistance = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (!isExploded)
        // {
        //     Debug.Log($"isExploded is false and we run mine split: {isExploded}");
        //     //SetIsExplodedOnFire(); // make sure this gets triggered before timer
        //     StartMineSplitTimer(); // start timer on mine splits...            
        // }
        
    }

    public override void OneTimeTriggerAfterProjectileFire()
    {

        // Only start the split if this specific instance hasn't been flagged
        if (!isExploded)
        {
            StartMineSplitTimer();
        }
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

    public void StartMineSplitTimer()
    {   
        StartCoroutine(MineSplit());
    }

    // delayed call, so mintes can be created once projectile is in the air...
    IEnumerator MineSplit()
    {
        isExploded = true;
        
        yield return new WaitForSeconds(mineSplitDelayTime);

        for(int i = 0; i < minesToSplitAmount; i++)
        {


            GameObject bullet = Instantiate(weaponData.projectilePreFab, gameObject.transform.position, gameObject.transform.rotation);


            // The Handoff: The Barrel gives the Projectile a reference to the data
            if (bullet.TryGetComponent(out BaseProjectile baseProjectileScript))
            {
                baseProjectileScript.SetIsExploded();
                // if (isExploded)
                // {
                //     baseProjectileScript.SetIsExplodedOnFire();
                // }
                baseProjectileScript.Setup(weaponData);
            }

            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // get rb for projectile
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // propel the projectile
            rb.AddForce(gameObject.transform.right * ((i + 1) *mineSeparationDistance), ForceMode2D.Impulse);
            
            
            
        }

        

        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
