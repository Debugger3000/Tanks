using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject controlsMenu;

    public AudioClip uiClick; // click on UI

    public AudioSource mainMenuSource; // in game music

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    public void StartGame()
    {
        Debug.Log("Start game Button Pressed!");
        // just load the game scenee
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Pressed!");
        Application.Quit(); // This works in the actual build, not the editor
    }

    public void DisplayControlPage()
    {
        Debug.Log("go to dislay page...");


        // load display screen...
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void DisplayToMainMenu()
    {
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ButtonClickSound()
    {
        mainMenuSource.PlayOneShot(uiClick);
    }


    // Update is called once per frame
//     void Update()
//     {
        
//     }
}
