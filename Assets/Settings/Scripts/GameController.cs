
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public static GameController Instance; // expose instance

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
    // public PlayerWeaponInventory p1Inventory;
    // public PlayerWeaponInventory p2Inventory;

    // hold UI weapon references + weapon Instances in general for each player...   
    public WeaponUI weaponUI;

    
    public float crateSpawnHeight = 500f;

    private TankBarrel player1Barrel;
    private TankBarrel player2Barrel;

    [Header("Starting Loadout")]
    public WeaponData starterWeapon1;
    public WeaponData starterWeapon2;


    [Header("Crates")]
    public CrateWeapons crateWeapons;
    public GameObject cratePrefab;
    public GameObject healthCratePrefab;
    public Transform p1CrateSpawn;
    public Transform p2CrateSpawn;

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


    // ---
    // INIT functions
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
    }

    public void InitPlayerInventory()
    {
        Debug.Log("INIT playerinventory from GameController...");

        // increment p2 start weapons
        weaponUI.IncrementWeapon(0, "HE-small");
        weaponUI.IncrementWeapon(0, "HE-large");
        weaponUI.IncrementWeapon(0, "Mines");
        // increment p2 start weapons
        weaponUI.IncrementWeapon(1, "HE-small"); // increment
        weaponUI.IncrementWeapon(1, "HE-large");
        weaponUI.IncrementWeapon(1, "Mines");


        // Set current tank weapons to default 
        WeaponInstance p1Start = weaponUI.GetWeapon(0, "HE-small");
        WeaponInstance p2Start = weaponUI.GetWeapon(1, "HE-small");
        player1Barrel.SetWeapon(p1Start);
        player2Barrel.SetWeapon(p2Start);
    }

    public void SetPlayerWeapon(int playerIndex, string weaponName)
    {
        WeaponInstance curWeaponInstance = weaponUI.GetWeapon(playerIndex, weaponName);
        // make sure return is not null
        if (curWeaponInstance != null)
        {
            Debug.Log($"Setting weapon for {playerIndex} to {curWeaponInstance.weaponData.weaponName}...");
            if(playerIndex == 0) player1Barrel.SetWeapon(curWeaponInstance);
            else player2Barrel.SetWeapon(curWeaponInstance);
        }
    }

    

    public void WeaponAmmoDecrement(int tankIndex, string weaponName)
    {
        // weapon fired, decrement this weapon
        weaponUI.DecrementWeapon(tankIndex, weaponName);
    }

    // ---
    // UI Logic
    // --> TankDamage UI
    public void TankDamage(int tankIndex, float currentHealth)
    {
        

        Debug.Log($"Tank {tankIndex} has  been damaged, current health argument now is:  {currentHealth}");
        float adjustedHealth = currentHealth / 100;
        Debug.Log($"Adjusted healh is: {adjustedHealth}");

        // healthPercent should be a value between 0 and 1
        if (tankIndex == 0)
            p1HealthBar.fillAmount = adjustedHealth;                                                     
        else
            p2HealthBar.fillAmount = adjustedHealth;
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
        Debug.Log($"Tank index: {tankIndex} just moved POWER to {powerPercent}");
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

    private void UpdateTurnUI()
    {
        //Debug.Log($"Tank {activePlayerIndex + 1}'s Turn");
        turnIndicator.text = $"Tank {activePlayerIndex + 1}'s Turn";
        // p1GasBar.fillAmount = 1.0f;
        // p2GasBar.fillAmount = 1.0f;
    }

    

    

    // ---
    // Turn Logic
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


    // ---
    // Crate Logic
    private void CrateSpawn()
    {
        System.Random random = new System.Random();
        // check if crate round
            // crate round every 3 rounds, it will flip flop between players...
        if(turnCounter > 0 && turnCounter % 3 == 0)
        {
            // determine spawn location for each crate
            int randX1 = random.Next(-67, -1);
            int randX2 = random.Next(1, 67);
            p1CrateSpawn.position = new Vector2(randX1, crateSpawnHeight);
            p2CrateSpawn.position = new Vector2(randX2, crateSpawnHeight);

            // determine health or weapon crate...
            int crateRoll1 = random.Next(0, 2);
            int crateRoll2 = random.Next(0, 2);

            // p1 crate instantiate
            if(crateRoll1 == 0)
            {
                // spawn weapons for p1
                Instantiate(cratePrefab, p1CrateSpawn.position, p1CrateSpawn.rotation);
            }else
            {
                Instantiate(healthCratePrefab, p1CrateSpawn.position, p1CrateSpawn.rotation); 
            }

            // p2 crate instantiate
            if(crateRoll2 == 0)
            {
                Instantiate(cratePrefab, p2CrateSpawn.position, p2CrateSpawn.rotation);
            }else
            {
                Instantiate(healthCratePrefab, p2CrateSpawn.position, p2CrateSpawn.rotation);
            }
            // play announcer audio
            AudioManager.Instance.PlayCrateInbound();
        }
    }

    public string GetRandomCrateWeapon()
    {
        System.Random random = new System.Random();
        List<WeaponData> weapons = crateWeapons.crateWeapons;
        int randomIndex = random.Next(0,weapons.Count);
        return weapons[randomIndex].weaponName; // return weapon name / id
    }

    // give tank weapon of the crate it has hit...
    public void TankHitsCrate(int tankIndex, string weaponName)
    {
        weaponUI.IncrementWeapon(tankIndex, weaponName);
    }

    // give tank health for health crate
    public void TankHitsHealthCrate(int tankIndex)
    {
        tankList[tankIndex].GiveHealth(); // give the tank health from crate..
        // Play heal audio
        AudioManager.Instance.PlayHealCrateSFX();
    }


    // ---
    // END GAME
    // End game, and display winner banner
    public void OnPlayerDeath(int losingPlayerIndex)
    {
        // deactivate 

        int winner = (losingPlayerIndex == 0) ? 1 : 2;
        winScreen.SetActive(true); // make end game UI active in hierarchy
        winText.text = $"PLAYER {winner} WINS!"; // set win text
    }
    public void RestartGame()
    {
        // Gets the index of the current scene and loads it again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); // load first scene in index... which is main menu
    }
    public void QuitApplication()
    {
        Application.Quit();
    }

}
