using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile01 : BaseProjectile
{
    //public float explosionRadius = 1f; // How many tiles to destroy
    // public GameObject hitEffectPrefab;

    // private WeaponData data;

    void Start()
    {
    }

    // Call this to pass in weapon data to determine 
    // public void Setup(WeaponData weaponData)
    // {
    //     data = weaponData;
    // }

    public override void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded)
    {
        // do nothing
        // no special effects on this tank weapon...
    }
}