using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mine : BaseProjectile
{
    protected int minesToSplitAmount = 3; // mines to break off

    public float mineSplitDelayTime = 2.5f;
    public float mineSeparationDistance = 5f;

    void Start()
    {
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
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate hitpoint / location rotation for explosion effect
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 hitPoint = contact.point;
        Vector2 normal = collision.contacts[0].normal;
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
        Quaternion hitRotation = Quaternion.Euler(0, 0, angle);

        if(collision.gameObject.layer == LayerMask.NameToLayer("Tanks"))
        {
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);  

            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Crate"))
        {
            GameObject effect = Instantiate(weaponData.hitEffectPrefab, hitPoint, hitRotation);
            // crate should be destroyed...
            Crate crate = collision.gameObject.GetComponent<Crate>();
            Destroy(crate);

            // Destroy the bullet itself
            Destroy(gameObject);
            Destroy(effect, 3f);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) 
        {
            ProjectileBounce(collision); // bounce projectile off wall
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

            // pass data to baseprojectile
            if (bullet.TryGetComponent(out BaseProjectile baseProjectileScript))
            {
                baseProjectileScript.SetIsExploded();
                baseProjectileScript.Setup(weaponData);
            }

            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            // get rb for projectile
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // propel the projectile
            rb.AddForce(gameObject.transform.right * ((i + -1) *mineSeparationDistance), ForceMode2D.Impulse);
        }
    }
}
