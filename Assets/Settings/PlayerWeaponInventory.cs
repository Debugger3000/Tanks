using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerWeaponInventory", menuName = "ScriptableObjects/PlayerWeaponInventory")]
public class PlayerWeaponInventory : ScriptableObject
{
    public List<WeaponInstance> ownedWeapons = new List<WeaponInstance>();

    // Key: string (Weapon Name), Value: WeaponInstance

    public void AddWeapon(WeaponInstance newWeapon)
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
