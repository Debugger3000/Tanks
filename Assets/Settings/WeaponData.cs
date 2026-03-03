using System.Collections.Generic;
using UnityEngine;

// This attribute allows you to right-click in the Project window to create a new weapon asset
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

    [Header("Child Projectile Stats")]
    public List<WeaponData> childWeaponData = new List<WeaponData>();

    // default is infinite
    // Pass diff value when creating weapon instance to change
    public int startAmmo = 9999;
    [Header("Stats")]
    // public float damage;
    // public float fireRate;
    // public int magazineSize;
    // public float reloadTime;
    public bool isMineType = false;

    [Header("Audio")]
    public AudioClip fireSound;

}
