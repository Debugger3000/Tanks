using UnityEngine;
using UnityEngine.Tilemaps;

// Basic projectile
public class Projectile01 : BaseProjectile
{
    void Start()
    {
    }

    public override void TriggerSpecialWeaponEffect(Vector2 hitPoint, Quaternion hitRotation, bool isExploded)
    {
        // do nothing
        // no special effects on this tank weapon...
    }
}