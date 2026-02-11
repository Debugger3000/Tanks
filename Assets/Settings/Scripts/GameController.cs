
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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

    public TankController[] tankList;

    private int activePlayerIndex = 0;
    private PlayerInput[] players;

    void Awake() { Instance = this; }

    public void InitializePlayers(PlayerInput p1, PlayerInput p2)
    {
        //Debug.Log("init''ing players.....");
        players = new PlayerInput[] { p1, p2 };
        UpdateTurnUI();
        SetInputFocus();
        turnCounter++;
        tankList[0].SetIsTurn(true);

        player1Barrel = p1.GetComponentInChildren<TankBarrel>();
        player2Barrel = p2.GetComponentInChildren<TankBarrel>();

        // set up player inventory with basic weapon
        InitPlayerInventory();
    }


    private void InitPlayerInventory()
    {
        p1Inventory.ResetInventory();
        p2Inventory.ResetInventory();

        // 2. Add the basic weapons to P1
        p1Inventory.AddWeapon(starterWeapon1);
        p1Inventory.AddWeapon(starterWeapon2);

        // 3. Add the basic weapons to P2
        p2Inventory.AddWeapon(starterWeapon1);
        p2Inventory.AddWeapon(starterWeapon2);

        // 4. (Optional) Set the tank's current weapon to the first starter
        player1Barrel.SetWeapon(starterWeapon1);
        player2Barrel.SetWeapon(starterWeapon1);
        
    }

    // public void ChangeActiveWeapon(WeaponData data)
    // {
    //     if (turnCounter % 2 != 0)
    //         player1Barrel.SetWeapon(data);
    //     else
    //         player2Barrel.SetWeapon(data);
    // }

    

    private void SetInputFocus()
    {
        // Turn off everyone, then turn on the active player
        players[0].DeactivateInput();
        players[1].DeactivateInput();
        players[activePlayerIndex].ActivateInput();
    }
    private void UpdateTurnUI()
    {
        //Debug.Log($"Tank {activePlayerIndex + 1}'s Turn");
        turnIndicator.text = $"Tank {activePlayerIndex + 1}'s Turn";
        p1GasBar.fillAmount = 1.0f;
        p2GasBar.fillAmount = 1.0f;
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

    public void SwitchTurn()
    {   
        Debug.Log($"switching turns now... {tankList.Length}");
        ChangeTankTurnLogic();
        turnCounter++; // increment turn...
        Invoke("SwitchTurnDelayed", turnDelay); // 
    }

    private void SwitchTurnDelayed()
    {
        // now change index
        activePlayerIndex = (activePlayerIndex == 0) ? 1 : 0;
        SetInputFocus();
        UpdateTurnUI();

        // check turns for crates...
        CrateSpawn();
    }

    private void ChangeTankTurnLogic()
    {
        // 1. Figure out who is NEXT before we reset anything
        int nextPlayerIndex = (activePlayerIndex == 0) ? 1 : 0;

        for (int i = 0; i < tankList.Length; i++)
        {
            bool isNext = (i == nextPlayerIndex);
            
            // Set the turn flag
            tankList[i].SetIsTurn(isNext);
            
            // ONLY reset gas for the tank whose turn is starting
            if (isNext)
            {
                tankList[i].ResetGas();
            }
        }
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
    
}
