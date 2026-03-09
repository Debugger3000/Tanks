using UnityEngine;
using UnityEngine.Tilemaps;

public class ClusterBomb : BaseProjectile
{
    public float clusterForce = 5f;

    public GameObject clusterPrefab;

    public override void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded)
    {
        if (!isExploded)
        {
            for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(clusterPrefab, hitPoint, hitRotation);

            // give base projectile data
            if (bullet.TryGetComponent(out BaseProjectile baseProjectileScript))
            {
                baseProjectileScript.Setup(weaponData.childWeaponData[0]); 
            }
            // grab a random angle for mini cluster to move on
            float randomAngle = Random.Range(250f, 310f);

            // This adds the random angle to the existing hitRotation
            bullet.transform.Rotate(0, 0, randomAngle);

            // make sure a tanks projectile doesn't explode on itself, on shoot
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            
            // get rigidbody
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // Push cluster mini out to a random direction
            rb.AddForce(bullet.transform.up * clusterForce, ForceMode2D.Impulse);
        }
        }
    }
}
