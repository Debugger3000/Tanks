using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrateWeapons", menuName = "ScriptableObjects/CrateWeapons")]
public class CrateWeapons : ScriptableObject
{
    public List<WeaponData> crateWeapons = new List<WeaponData>();

    // Helper function to get a random weapon from the list
    public WeaponData GetRandomWeapon()
    {
        if (crateWeapons.Count == 0) return null;
        int randomIndex = Random.Range(0, crateWeapons.Count);
        return crateWeapons[randomIndex];
    }
}
