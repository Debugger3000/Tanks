using System.Data.Common;
using UnityEngine;

// WeaponInstance class 
// Hold WeaponData + other data about weapon in inventory
// Ammo
[System.Serializable]
public class WeaponInstance
{
    // hold weaponData
    public WeaponData weaponData; 

    // ammo in inventory instance...
    public int currentAmmo;

    // whether weapon is active or not
    // show gray or don't equip unactive weapons since the player doesn't have them yet
    public bool active = false;

    // corresponding UI button for this weapon instance
    public GameObject button;

    // Give weaponData + ammo to carry
    public WeaponInstance(WeaponData data)
    {
        weaponData = data;
        currentAmmo = 0;
    }

    // Increment weapon ammo
    public void SetAmmo(int incrementAmount)
    {
        currentAmmo += incrementAmount; // increment ammo
    }
}
