using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public PlayerWeaponInventory inventory; // Reference to your SO
    //public GameObject buttonPrefab;   // A button with a text component
    // public TankBarrel playerBarrel;
    private GameController gameController;
    public WeaponData weapon;

    [SerializeField]
    public int playerIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the controller once at the start
        gameController = FindAnyObjectByType<GameController>();
    }

    // call gamecontroller method to set weapon for whatever player...
    public void OnClickSetWeapon()
    {
        Debug.Log("BUTTON CLICKED BUITTON CLIEDK");
        // make sure only player 1 buttons set player 1's items
        gameController.SetPlayerWeapon(playerIndex, weapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OnClick()
    // {
    //     //gameController.ChangeActiveWeapon(weapon);
    // }
}
