using System.Data.Common;
using UnityEngine;

// WeaponInstance class 
// Hold WeaponData + other data about weapon in inventory
// Ammo
[System.Serializable]
public class WeaponUIInstance
{
    // This connects the instance to the ScriptableObject
    public WeaponData weaponData;
    public string weaponNameID = "HE-small";
    public GameObject uiWeaponButton;
    // components of UI button
    
    // public TMPro.TextMeshProUGUI ammoText;

   


    // Give weaponData + ammo to carry
    public WeaponUIInstance(WeaponData data, GameObject button)
    {
        weaponData = data; // set the actual weapon data
        uiWeaponButton = button; // set weapon icon button
    }


}
