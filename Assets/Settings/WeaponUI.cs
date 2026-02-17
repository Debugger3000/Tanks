using UnityEngine;
using System.Collections.Generic;

// [CreateAssetMenu(fileName = "WeaponUIRegistry", menuName = "UI/Weapon UI Registry")]
public class WeaponUI : MonoBehaviour
{
    // list of Player UI weapon controls
    // 0 - 1 indexed lists
    // holds weaponUIInstances to grab and alter for players
    public List<WeaponUIInstance> weaponUIInstances = new List<WeaponUIInstance>();

    // 
    public string nameWeapon = "john";

    // Helper function to turn everything off
    public void ResetAllIcons()
    {
        // foreach (var slot in weaponUIInstances)
        // {
        //     if (slot.greenIcon != null) 
        //         slot.greenIcon.gameObject.SetActive(false);
        // }
    }
}