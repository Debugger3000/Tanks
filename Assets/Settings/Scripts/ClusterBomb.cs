using UnityEngine;
using UnityEngine.Tilemaps;

public class ClusterBomb : BaseProjectile
{
    public float clusterForce = 5f;

    public GameObject clusterPrefab;

    public override void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded)
    {
        // Add explosion logic here
        Debug.Log("BOOM! Creating explosion particles.");

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
           //isExploded = true; // make sure we only trigger mini clusters once... 
        }
    }
}
