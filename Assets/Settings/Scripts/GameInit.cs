using UnityEngine;
using UnityEngine.InputSystem;

public class GameInit : MonoBehaviour
{
    public GameObject T1; // Drag your Tank Prefab here
    public GameObject T2;
    public Transform spawnPoint1;   // Create an empty GO for spawn position
    public Transform spawnPoint2;

    void Start()
    {
        // 1. Spawn Player 1
        // This creates the object AND assigns the Index and Scheme in one go
        var p1 = PlayerInput.Instantiate(T1, 
            playerIndex: 0, 
            controlScheme: "T1", 
            pairWithDevices: new InputDevice[] { Keyboard.current });
        
        p1.transform.position = spawnPoint1.position;

        // 2. Spawn Player 2
        var p2 = PlayerInput.Instantiate(T2, 
            playerIndex: 1, 
            controlScheme: "T1", 
            pairWithDevices: new InputDevice[] { Keyboard.current });
        
        p2.transform.position = spawnPoint2.position;

        p1.ActivateInput();
        p2.ActivateInput();

        Debug.Log($"Spawned P1 (Index: {p1.playerIndex}) and P2 (Index: {p2.playerIndex})");

        // Link them to the controller
        GameController.Instance.InitializePlayers(p1, p2);

        
    }
}