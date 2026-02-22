using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

// [CreateAssetMenu(fileName = "WeaponUIRegistry", menuName = "UI/Weapon UI Registry")]
public class WeaponUI : MonoBehaviour
{
    // holds weaponUIInstances to grab and alter for players
    public List<WeaponInstance> p1Weapons = new List<WeaponInstance>(); // use this to populate from unity ui

    public List<WeaponInstance> p2Weapons = new List<WeaponInstance>(); // use this to populate from unity ui
    
    public List<List<WeaponInstance>> allPlayerWeapons = new List<List<WeaponInstance>>();


    void Start()
    {
        // populate player array for easy traversal of pWeapons based on index
        allPlayerWeapons.Add(p1Weapons); // Index 0
        allPlayerWeapons.Add(p2Weapons); // Index 1
        Debug.Log($"allPlayerWeapons length first call is: {allPlayerWeapons.Count}");
        GameController.Instance.InitPlayerInventory();

        Debug.Log($"allPlayerWeapons length: {allPlayerWeapons.Count}");

        Debug.Log($"player index 1: weapon1 is : {allPlayerWeapons[1][0].weaponData.weaponName} & weapon 2 is : {allPlayerWeapons[1][1].weaponData.weaponName}");

        // set ammo amounts for starting weapons for both players
        // done via GameController

    }

    // Helper function to turn everything off
    public void ResetAllIcons()
    {
        // foreach (var slot in weaponUIInstances)
        // {
        //     if (slot.greenIcon != null) 
        //         slot.greenIcon.gameObject.SetActive(false);
        // }
    }

    private WeaponInstance GrabWeaponLocal(int playerIndex, string weaponName)
    {
        //Debug.Log($"index for grab weapon local is: {playerIndex}");
        foreach (WeaponInstance weapon in allPlayerWeapons[playerIndex])
        {
            if(weapon.weaponData.weaponName == weaponName)
            {
                return weapon;
            }
        }
        return null;
    }

    // return weaponData based on weaponName (unique)
    public WeaponInstance GetWeapon(int playerIndex, string weaponName)
    {
        if (playerIndex > 1) return null;
        WeaponInstance instance = GrabWeaponLocal(playerIndex,weaponName);
        if(instance != null) return instance;
        else return null;
    }

    public bool IsWeaponActive(int playerIndex, string weaponName)
    {
        if (playerIndex > 1) return false;
        WeaponInstance instance = GrabWeaponLocal(playerIndex,weaponName);
        if(instance.active) return true;
        else return false;
    }

    // get Button from weaponInstance to do something with
    public GameObject GetButton(int playerIndex, string weaponName)
    {
      if (playerIndex > 1) return null;
        WeaponInstance instance = GrabWeaponLocal(playerIndex,weaponName);
        if(instance != null) return instance.button;
        else return null;
    }

    // selecting button so make its icon active
    // make other players weapon icons deactive
    public void SelectButton(int playerIndex, GameObject buttonClicked)
    {
        if (playerIndex > -1 && playerIndex < 2)
        {
            foreach(WeaponInstance curInstance in allPlayerWeapons[playerIndex])
            {
                GameObject curButton = curInstance.button;
                // Look for the icon child in every button
                Transform icon = curButton.transform.Find("Icon");
            
                if (icon != null)
                {
                    Debug.Log($"Setting icon to active for {curInstance.weaponData.weaponName}");
                    // If it's the button we clicked, turn it ON. Otherwise, OFF.
                    icon.gameObject.SetActive(curButton == buttonClicked);
                }
            }
        }
        else
        {
            //do nothing
        }
        
    }


    // this is where we want to change ammo drops and active game weapon states
    // so we can call inventory method from here 
    public void DecrementWeapon(int playerIndex, string weaponName)
    {
        // grab weapon instance
        if (playerIndex > -1 && playerIndex < 2)
        {
            WeaponInstance instance = GrabWeaponLocal(playerIndex,weaponName);
            int newAmmoCount = instance.currentAmmo - 1; //decrement ammo 
            instance.currentAmmo = newAmmoCount; // set new amount to instance

            if (newAmmoCount < 1)
            {
                // if ammo zero, we need to deactivate
                MakeWeaponDeActive(instance);
            }
            SetAmmoUI(instance, newAmmoCount); // set UI for ammo text
        }
    }

    public void IncrementWeapon(int playerIndex, string weaponName)
    {
        // grab weapon instance
        if (playerIndex > -1 && playerIndex < 2)
        {
            WeaponInstance instance = GrabWeaponLocal(playerIndex,weaponName);

            int maxAmmo = instance.weaponData.startAmmo; // grab ammo amount

            if (instance.currentAmmo == 0)
            {
                // if ammo zero, we need to deactivate
                MakeWeaponActive(instance);
            }
            int ammoAmount = instance.currentAmmo + maxAmmo; // get new ammo amount
            instance.currentAmmo = ammoAmount; // set currentAmmo with new amount
            SetAmmoUI(instance, ammoAmount); // set UI for ammo text
        }
    }

    public void MakeWeaponActive(WeaponInstance weaponInstance)
    {
        weaponInstance.active = true;
        // call inventory to remove gray
        WhiteOut(weaponInstance);
    }

    public void MakeWeaponDeActive(WeaponInstance weaponInstance)
    {
        weaponInstance.active = false;
        
        // call inventory to gray out image and make ammo text ""
        GrayOut(weaponInstance); // gray out weapon icon
    }
    private void SetAmmoUI(WeaponInstance weaponInstance, int amount)
    {
        Debug.Log($"Setting ammo for {weaponInstance.weaponData.weaponName} with ammo amount of: {amount}");
        // grab text element
        TextMeshProUGUI textElement = weaponInstance.button.GetComponentInChildren<TextMeshProUGUI>();
        
        if (textElement != null)
        {
            if(amount != 0) textElement.text = amount.ToString(); // set to amount above 0
            else textElement.text = ""; // set to nothing cause we are graying it out if 0
        }
        else
        {
            Debug.Log($"Text mesh pro is null somehow...........................");
        }
    }

    private void GrayOut(WeaponInstance weaponInstance)
    {
        Image buttonImage = weaponInstance.button.GetComponent<Image>();

        if (buttonImage != null)
        {
            // 2. Set the color to gray
            buttonImage.color = Color.gray5; 
        }
    }

    private void WhiteOut(WeaponInstance weaponInstance)
    {
        Image buttonImage = weaponInstance.button.GetComponent<Image>();

        if (buttonImage != null)
        {
            // 2. Set the color to gray
            buttonImage.color = Color.white; 
        }
    }



    
}