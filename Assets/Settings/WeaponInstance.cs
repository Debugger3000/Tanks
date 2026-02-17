using System.Data.Common;
using UnityEngine;

// WeaponInstance class 
// Hold WeaponData + other data about weapon in inventory
// Ammo
[System.Serializable]
public class WeaponInstance
{
    // This connects the instance to the ScriptableObject
    public WeaponData weaponData; 

    // ammo in inventory instance...
    public int currentAmmo;


    // Give weaponData + ammo to carry
    public WeaponInstance(WeaponData data)
    {
        weaponData = data;
        currentAmmo = data.startAmmo;
    }

    // Increment weapon ammo
    public void SetAmmo(int incrementAmount)
    {
        currentAmmo += incrementAmount; // increment ammo
    }
}
