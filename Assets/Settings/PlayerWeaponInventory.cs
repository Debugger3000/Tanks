using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerWeaponInventory", menuName = "ScriptableObjects/PlayerWeaponInventory")]
public class PlayerWeaponInventory : ScriptableObject
{
    public List<WeaponData> ownedWeapons = new List<WeaponData>();

    public void AddWeapon(WeaponData newWeapon)
    {
        //
        if (!ownedWeapons.Contains(newWeapon))
        {
            ownedWeapons.Add(newWeapon);
        }
    }

    // clear inventory...
    public void ResetInventory()
    {
        ownedWeapons.Clear();
    }
}
