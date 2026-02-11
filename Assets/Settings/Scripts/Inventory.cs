using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public PlayerWeaponInventory inventory; // Reference to your SO
    //public GameObject buttonPrefab;   // A button with a text component
    // public TankBarrel playerBarrel;
    private GameController gameController;
    public WeaponData weapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find the controller once at the start
        gameController = FindAnyObjectByType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        //gameController.ChangeActiveWeapon(weapon);
    }
}
