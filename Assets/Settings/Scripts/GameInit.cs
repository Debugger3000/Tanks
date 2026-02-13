using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameInit : MonoBehaviour
{
    public GameObject T1; // Drag your Tank Prefab here
    public GameObject T2;
    // public GameObject mouseObject;
    public Transform spawnPoint1;   // Create an empty GO for spawn position
    public Transform spawnPoint2;

    void Start()
    {

        InputDevice keyboard = Keyboard.current;
        InputDevice mouse = Mouse.current;

        // Fallback: If .current is null, try to find ANY device of that type
        if (keyboard == null) keyboard = InputSystem.GetDevice<Keyboard>();
        if (mouse == null) mouse = InputSystem.GetDevice<Mouse>();


        if(mouse == null)
        {
            Debug.Log("mouse is still null somehow ???");
        }
        // Create the list safely
        var devices = new InputDevice[]{keyboard, mouse};
        // if (keyboard != null) devices.Add(keyboard);
        // if (mouse != null) devices.Add(mouse);

        // if (devices.Length < 2)
        // {
        //     Debug.LogError("Device list is less than 2...");
        //     return;
        // }

        //instantiate players in game and their input
        var p1 = PlayerInput.Instantiate(T1, 
            playerIndex: 0, 
            controlScheme: "T1",
            pairWithDevices: devices);

        // var p1 = PlayerInput.Instantiate(T1, 
        //     playerIndex: 0);
        
        // ConfigurePlayerUI(p1);
        p1.transform.position = spawnPoint1.position;
        

        
        var p2 = PlayerInput.Instantiate(T2, 
            playerIndex: 1,
            controlScheme: "T2",
            pairWithDevices: devices);
        //ConfigurePlayerUI(p2);
        p2.transform.position = spawnPoint2.position;
       

        // var mouse = PlayerInput.Instantiate(mouseObject, 
        //     playerIndex: 2, 
        //     controlScheme: "Mouse", 
        //     pairWithDevice: Mouse.current);

        //p1.ActivateInput();
        //p2.ActivateInput();
        //mouse.ActivateInput();

        //Debug.Log($"Spawned P1 (Index: {p1.playerIndex}) and P2 (Index: {p2.playerIndex})");

        // Link them to the controller
        GameController.Instance.InitializePlayers(p1, p2); 


        // void ConfigurePlayerUI(PlayerInput player)
        // {
        //     // Find the MultiplayerEventSystem on the instantiated player prefab
        //     var eventSystem = player.GetComponentInChildren<MultiplayerEventSystem>();
        //     var uiModule = player.GetComponentInChildren<InputSystemUIInputModule>();

        //     if (uiModule != null)
        //     {
        //         // Explicitly map the UI actions from the player's local input asset
        //         uiModule.actionsAsset = player.actions; 
                
        //         // This ensures the UI module knows which "Player" it belongs to
        //         player.uiInputModule = uiModule; 
        //     }
        // }
    }
}