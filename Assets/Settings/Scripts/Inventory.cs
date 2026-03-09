using System.Collections.Generic;
using UnityEngine;

// Class InventoryUI
public class Inventory : MonoBehaviour
{
    // these references are not necessary since they are in Gamecontroller.Instance but whatever
    private GameController gameController;
    private WeaponUI weaponUI; // reference to weaponUI

    public string weaponName = "HE-small";


    [SerializeField]
    public int playerIndex = 0;
    void Start()
    {
        // set weaponUI so we can communicate with it
        weaponUI = FindAnyObjectByType<WeaponUI>();
        // Find the controller once at the start
        gameController = FindAnyObjectByType<GameController>();
    }

    // call gamecontroller method to set weapon for whatever player...
    public void OnClickSetWeapon()
    {
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
    }

    public void GameMenuClick()
    {
        GameController.Instance.OpenGameMenu(); // open game menu...
        GameController.Instance.GameMenuOpenedDisableControls(); // disable player controls...
    }

}
