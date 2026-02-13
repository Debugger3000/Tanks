using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameInit : MonoBehaviour
{
    public GameObject T1; // Drag your Tank Prefab here
    public GameObject T2;
    public GameObject mouseObject;
    public Transform spawnPoint1;   // Create an empty GO for spawn position
    public Transform spawnPoint2;

    void Start()
    {
        // instantiate players in game and their input
        var p1 = PlayerInput.Instantiate(T1, 
            playerIndex: 0, 
            controlScheme: "T1", 
            pairWithDevices: new InputDevice[] { Keyboard.current });
        
        p1.transform.position = spawnPoint1.position;

        
        var p2 = PlayerInput.Instantiate(T2, 
            playerIndex: 1, 
            controlScheme: "T1", 
            pairWithDevices: new InputDevice[] { Keyboard.current });

        p2.transform.position = spawnPoint2.position;

        var mouse = PlayerInput.Instantiate(mouseObject, 
            playerIndex: 2, 
            controlScheme: "Mouse", 
            pairWithDevice: Mouse.current);


        // FIND the EventSystem and tell it: "Use THIS player for UI"
        var uiModule = FindFirstObjectByType<InputSystemUIInputModule>();
        if (uiModule != null)
        {
            Debug.Log("assign mouse to input system");
            uiModule.actionsAsset = mouse.actions; // Use the mouse's actions
            // uiModule.unassignActionsOnStop = false;
        }

        p1.ActivateInput();
        //p2.ActivateInput();
        mouse.ActivateInput();

        Debug.Log($"Spawned P1 (Index: {p1.playerIndex}) and P2 (Index: {p2.playerIndex})");

        // Link them to the controller
        GameController.Instance.InitializePlayers(p1, p2); 
    }
}