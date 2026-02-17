
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI turnIndicator;
    public GameObject winScreen;
    public TextMeshProUGUI winText;
    public Image p1HealthBar;
    public Image p2HealthBar;
    public Image p1GasBar;
    public Image p2GasBar;
    

    public Image p1PowerBar;
    public TextMeshProUGUI p1PowerText;
    public Image p2PowerBar;
    public TextMeshProUGUI p2PowerText;


    [Header("Player Inventory")]
    [Header("Inventory Data")]
    public PlayerWeaponInventory p1Inventory;
    public PlayerWeaponInventory p2Inventory;

    private TankBarrel player1Barrel;
    private TankBarrel player2Barrel;

    [Header("Starting Loadout")]
    public WeaponData starterWeapon1;
    public WeaponData starterWeapon2;

    [Header("Game Settings")]
    readonly private float turnDelay = 5.0f;
    private int turnCounter = 0;

    // Players(Tanks) 1 & 2 - script list
    private TankController[] tankList;
    // Players(Tanks) 1 & 2 - InputSystem.PlayerInput list
    private PlayerInput[] players;
    // current Player index (Tank 0 & 1)
    private int activePlayerIndex = 0;
    
    // Safeguard 
    private bool isSwitching = false;

    // public InputActionAsset myInputActions;

    // expose GameController via GameController.Instance
    void Awake() { 
        Instance = this;
        }


    public void InitializePlayers(PlayerInput p1, PlayerInput p2)
    {
        Debug.Log("GameController INIT");
        // myActions.UI.Enable(); // enable UI

        players = new PlayerInput[] { p1, p2 };

        // grab player instances into tankList for TankController
        tankList = new TankController[] {
            p1.GetComponent<TankController>(),
            p2.GetComponent<TankController>()
        };
        // grab player instances into their barrel variables
        player1Barrel = p1.GetComponentInChildren<TankBarrel>();
        // player1Barrel.gameObject.GetComponent<Renderer>().material.color = Color.red;
        player2Barrel = p2.GetComponentInChildren<TankBarrel>();
        
        Debug.Log($"Tank list is: {tankList}");

        // Start game
        UpdateTurnUI();
        DeactivateInput();
        SetCurrentTurnFocus(); // activate current players input
        NewPlayersTurn(); // set players values to appriproate values for their turn 

        // set up player inventory with basic weapon
        InitPlayerInventory();
    }


    private void InitPlayerInventory()
    {
        //p1Inventory.ResetInventory();
        //p2Inventory.ResetInventory();

        // Add defaults weapons to Player 1 
        p1Inventory.AddWeapon(new WeaponInstance(starterWeapon1));
        p1Inventory.AddWeapon(new WeaponInstance(starterWeapon2));

        // Add defaults weapons to Player 2
        p2Inventory.AddWeapon(new WeaponInstance(starterWeapon1));
        p2Inventory.AddWeapon(new WeaponInstance(starterWeapon2));

        // 4. (Optional) Set the tank's current weapon to the first starter
        player1Barrel.SetWeapon(new WeaponInstance(starterWeapon1));
        player2Barrel.SetWeapon(new WeaponInstance(starterWeapon2));
    }

    public void SetPlayerWeapon(int playerIndex, string weaponData)
    {
        Debug.Log($"Setting {playerIndex} weapon to {weaponData}");
        if (playerIndex == 0)
        {
            foreach (WeaponInstance weapon in p1Inventory.ownedWeapons)
            {   // find weapon that UI correlates too
                if(weapon.weaponData.name == weaponData){
                    player1Barrel.SetWeapon(weapon); // set weapon 
                }
            }
        }   
        else
        {
            foreach (WeaponInstance weapon in p1Inventory.ownedWeapons)
            {   // find weapon that UI correlates too
                if(weapon.weaponData.name == weaponData){
                    player2Barrel.SetWeapon(weapon); // set weapon 
                }
            }   
        }
        
    }
    
    private void UpdateTurnUI()
    {
        //Debug.Log($"Tank {activePlayerIndex + 1}'s Turn");
        turnIndicator.text = $"Tank {activePlayerIndex + 1}'s Turn";
        // p1GasBar.fillAmount = 1.0f;
        // p2GasBar.fillAmount = 1.0f;
    }


    public void TankDamage(int tankIndex, float healthPercent)
    {
        Debug.Log($"Tank {tankIndex} has  been damaged");
        float adjustedHealth = healthPercent / 100;
        // healthPercent should be a value between 0 and 1
        if (tankIndex == 0)
            p1HealthBar.fillAmount = adjustedHealth;                                                     
        else
            p2HealthBar.fillAmount = adjustedHealth;
    }

    public void OnPlayerDeath(int losingPlayerIndex)
    {
        int winner = (losingPlayerIndex == 0) ? 2 : 1;
        winScreen.SetActive(true);
        winText.text = $"PLAYER {winner} WINS!";
        
        // Disable all input so they can't move after the game ends
        players[0].DeactivateInput();
        players[1].DeactivateInput();
    }


    // called by TankController after their shot
    public void SwitchTurn()
    {   
        if (isSwitching) return; // Block the second call!
        isSwitching = true;
        StartCoroutine(SwitchTurnDelayed());
    
        //Invoke("SwitchTurnDelayed", turnDelay); // 
    }

    // delayed call, so animations can play out 
    IEnumerator SwitchTurnDelayed()
    {
        isSwitching = true;
        DeactivateInput();
        yield return new WaitForSeconds(turnDelay);

        Debug.Log($"switching turns now... old index {activePlayerIndex}");
        
        Debug.Log($"switching turns now... new index {activePlayerIndex}");

        EndOfPlayerTurn(); // reset end of players turn to values
        NewPlayersTurn(); // set new players turn values

        UpdateTurnUI(); // switch turn indicator
        CrateSpawn(); // check turns for crates...
        SetCurrentTurnFocus();
        isSwitching = false;
    }

    // control device activation per player
    private void DeactivateInput()
    {
        players[0].DeactivateInput();
        //players[0].enabled = false;
        //players[0].currentActionMap.Disable();
        players[1].DeactivateInput();
        //players[1].enabled = false;
        //players[1].currentActionMap.Disable();
         // turn on active player 
    }

    private void SetCurrentTurnFocus()
    {
        //players[activePlayerIndex].enabled = true;
        players[activePlayerIndex].ActivateInput();
        //OnTurnSwap(players[activePlayerIndex]);
        // players[activePlayerIndex].currentActionMap.Enable();
    }

    private void EndOfPlayerTurn()
    {
        tankList[activePlayerIndex].SetIsTurn(false); // set end of players turn
        tankList[activePlayerIndex].GetComponentInChildren<TankBarrel>().SetHasPlayerShot(false);
    }

    private void NewPlayersTurn()
    {
        turnCounter++; // increment turn...
        if(turnCounter != 1)
        {
            activePlayerIndex = (activePlayerIndex == 0) ? 1 : 0; // now change index            
        }
        
        // set new turn players turn to true
        tankList[activePlayerIndex].SetIsTurn(true);
        tankList[activePlayerIndex].ResetGas(); // set gas to full
        SetCurrentTurnFocus(); // activate input for current turn player
        Debug.Log($"Activeturnindex: {activePlayerIndex} - turncounter: {turnCounter} - Set to true");
    }

    // deal with Gas UI changes
    public void TankGas(int tankIndex, float gasPercent)
    {
        float adjustedGas = gasPercent / 100f;
        // healthPercent should be a value between 0 and 1
        if (tankIndex == 0)
            p1GasBar.fillAmount = adjustedGas;                                                     
        else
            p2GasBar.fillAmount = adjustedGas;
    }

    // deal with power bar UI changes
    public void SetPowerBar(int tankIndex, float powerPercent)
    {
        Debug.Log($"Tank index: {tankIndex} just moved POWER");
        // float adjustedPower = powerPercent / 100f;
        if(tankIndex == 0)
        {
            p1PowerBar.fillAmount = powerPercent;
            // change text percent too...
            p1PowerText.text = $"{powerPercent * 100}%";
        }
        else
        {
            p2PowerBar.fillAmount = powerPercent;
            p2PowerText.text = $"{powerPercent * 100}%";
        }
    } 

    private void CrateSpawn()
    {
        // spawn first crate on turn 7
        if (turnCounter == 7)
        {
            // spawn two crates for each player
        }
    }


//     public void OnTurnSwap(PlayerInput newActivePlayer)
//     {
//     // 1. Get the components from your EventSystem object
//     var multiEvent = FindFirstObjectByType<MultiplayerEventSystem>();
//     var uiModule = FindFirstObjectByType<InputSystemUIInputModule>();

//     if (multiEvent != null && uiModule != null)
//     {
//         // 2. Point the UI Module to the new player's actions
//         // This makes the mouse 'Point' and 'Click' work for the new turn
//         uiModule.actionsAsset = newActivePlayer.actions;

//         // 3. Link the PlayerInput to this specific UI Module
//         newActivePlayer.uiInputModule = uiModule;

//         // 4. Update the Player Root
//         // If your UI is global, set this to the Canvas. 
//         // If each tank has its own overhead UI, set it to the tank.
//         multiEvent.playerRoot = newActivePlayer.gameObject;

//         // 5. Optional: Auto-select a button for the new player
//         // multiEvent.SetSelectedGameObject(someDefaultButton);
//     }
// }
    
}
