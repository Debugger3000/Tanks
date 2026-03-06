using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniClusterBomb : BaseProjectile
{
    //public float explosionRadius = 1f; // How many tiles to destroy
    // public GameObject hitEffectPrefab;

    //private WeaponData data;

    // public float clusterForce = 5f;

    // public float damage = 5f;
    // public float explosionRadius = 0.5f;

    // public GameObject clusterPrefab;

    // Call this to pass in weapon data to determine 
    // public void Setup(WeaponData weaponData)
    // {
    //     data = weaponData;
    // }

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("SpawnedProjectiles");
        Invoke("EnableCollision", 0.5f);
    }

    public override void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded)
    {
        // do nothing
        // no special effects on this tank weapon...
    }

    void EnableCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("Projectiles");
    }
}
