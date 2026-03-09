using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

// First script that runs on game launch
// Possibly might not be needed after I figured out input issues via 2 players on 1 device...
// So this is just kept here for now lol... 
public class GameInit : MonoBehaviour
{
    public GameObject T1; // Drag your Tank Prefab here
    public GameObject T2;
    // public GameObject mouseObject;
    public Transform spawnPoint1;   // Create an empty GO for spawn position
    public Transform spawnPoint2;

    void Start()
    {
        // devices
        InputDevice keyboard = Keyboard.current;
        InputDevice mouse = Mouse.current;

        // fallback
        if (keyboard == null) keyboard = InputSystem.GetDevice<Keyboard>();
        if (mouse == null) mouse = InputSystem.GetDevice<Mouse>();


        if(mouse == null)
        {
            Debug.Log("mouse is still null somehow ???");
        }
        // Create the list of devices...
        var devices = new InputDevice[]{keyboard, mouse};
    
        // Player 1 set up
        var p1 = PlayerInput.Instantiate(T1, 
            playerIndex: 0, 
            controlScheme: "T1",
            pairWithDevices: devices);
        
        p1.transform.position = spawnPoint1.position;
        
        // Player 2 set up
        var p2 = PlayerInput.Instantiate(T2, 
            playerIndex: 1,
            controlScheme: "T2",
            pairWithDevices: devices);
        p2.transform.position = spawnPoint2.position;
    
        // Link them to the controller
        GameController.Instance.InitializePlayers(p1, p2); 
    }
}