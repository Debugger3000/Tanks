using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject controlsMenu;

    public AudioClip uiClick; // click on UI

    public AudioSource mainMenuSource; // in game music

    public void StartGame()
    {
        // just load the game scenee
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit(); // This works in the actual build, not the editor
    }

    public void DisplayControlPage()
    {
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
}
