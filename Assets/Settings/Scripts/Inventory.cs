using System.Collections.Generic;
using UnityEngine;

// Class InventoryUI
public class Inventory : MonoBehaviour
{
    //public PlayerWeaponInventory inventory; // Reference to your SO
    //public GameObject buttonPrefab;   // A button with a text component
    // public TankBarrel playerBarrel;
    private GameController gameController;
    public WeaponData weaponData;

    // public List<string> weaponNames = new List<string> { "HE-small", "HE-large" };
    // icons for certain player... 
    public GameObject[] allButtons;
    
    public string iconName = "Icon"; 

    public TMPro.TextMeshProUGUI ammoText;

    [SerializeField]
    public int playerIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the controller once at the start
        gameController = FindAnyObjectByType<GameController>();
        // set ammo font text...
        if(weaponData.startAmmo > 10)
        {
            ammoText.text = "99";            
        }
        else
        {
            Debug.Log(weaponData.startAmmo.ToString());
            ammoText.text = weaponData.startAmmo.ToString();
        }
    }

    // call gamecontroller method to set weapon for whatever player...
    public void OnClickSetWeapon()
    {
        Debug.Log("BUTTON CLICKED BUITTON CLIEDK");
        // make sure only player 1 buttons set player 1's items
        gameController.SetPlayerWeapon(playerIndex, weaponData.name);
    }

    // control current weapon Active icon UI for players
    public void SelectButton(GameObject clickedButton)
    {
        foreach (GameObject btn in allButtons)
        {
            // Look for the icon child in every button
            Transform icon = btn.transform.Find(iconName);
            
            if (icon != null)
            {
                // If it's the button we clicked, turn it ON. Otherwise, OFF.
                icon.gameObject.SetActive(btn == clickedButton);
            }
        }
    }


}
