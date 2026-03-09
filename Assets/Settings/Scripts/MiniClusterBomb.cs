using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniClusterBomb : BaseProjectile
{
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
