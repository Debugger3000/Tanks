using System.Collections.Generic;
using UnityEngine;

// Where projectile scriptable data is created from
[CreateAssetMenu(fileName = "TankWeapon", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{

    // This is more appropriately Projectile / Tank Shell data
        // we change projectiles

    [Header("Visuals")]
    public string weaponName;
    // public GameObject weaponPrefab;
    public Sprite icon;
    public GameObject projectilePreFab;
    public GameObject hitEffectPrefab;
    public float damage = 15;
    // default is 1f radius, two holes in terrain have to be made close together to disrupt tank movement
    public float explosionRadius = 1f; // How many tiles to destroy

    public float explosiveDamage = 10f; // Near hit damage
    public float explosiveBuffer = 3f; // explosionradius + buffer = explosive raycast zone for nearhits 

    [Header("Child Projectile Stats")]
    public List<WeaponData> childWeaponData = new List<WeaponData>();

    public int startAmmo = 9999;
    [Header("Stats")]
    
    public bool isMineType = false;

    [Header("Audio")]
    public AudioClip fireSound;

}
