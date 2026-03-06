using System.Collections.Generic;
using UnityEngine;

// Class InventoryUI
public class Inventory : MonoBehaviour
{
    //public PlayerWeaponInventory inventory; // Reference to your SO
    //public GameObject buttonPrefab;   // A button with a text component
    // public TankBarrel playerBarrel;
    private GameController gameController;

    private WeaponUI weaponUI;
    // public WeaponData weaponData;

    public string weaponName = "HE-small";

    // private WeaponInstance weaponInstance;

    // public List<string> weaponNames = new List<string> { "HE-small", "HE-large" };
    // icons for certain player... 
    // public GameObject[] allButtons;
    
    // public string iconName = "Icon"; 

    // public TMPro.TextMeshProUGUI ammoText;

    [SerializeField]
    public int playerIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // set weaponUI so we can communicate with it
        weaponUI = FindAnyObjectByType<WeaponUI>();

        // Find the controller once at the start
        gameController = FindAnyObjectByType<GameController>();

        // set weaponInstance reference
        //weaponInstance = weaponUI.GetWeapon(playerIndex,weaponName);
    }

    // call gamecontroller method to set weapon for whatever player...
    public void OnClickSetWeapon()
    {
        Debug.Log("BUTTON CLICKED BUITTON CLIEDK");

        // make sure weapon is available...
        if(weaponUI.IsWeaponActive(playerIndex, weaponName))
        {
            // set that players weapon to that
            // call to GameController because we have player barrel reference there
            gameController.SetPlayerWeapon(playerIndex, weaponName);
        }
    }

    // control current weapon Active icon UI for players
    public void SelectButton(GameObject clickedButton)
    {
        // check GameObject reference ID
        if (weaponUI.IsWeaponActive(playerIndex, weaponName))
        {
            weaponUI.SelectButton(playerIndex,clickedButton); // deselect all other buttons
        }

        // foreach (GameObject btn in allButtons)
        // {
        //     // Look for the icon child in every button
        //     Transform icon = btn.transform.Find(iconName);
            
        //     if (icon != null)
        //     {
        //         // If it's the button we clicked, turn it ON. Otherwise, OFF.
        //         icon.gameObject.SetActive(btn == clickedButton);
        //     }
        // }
    }

    public void GameMenuClick()
    {
        GameController.Instance.OpenGameMenu(); // open game menu...
        Debug.Log($"We clicked menu icon on main UI");
    }

}
