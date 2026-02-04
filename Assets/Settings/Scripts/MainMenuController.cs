using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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


    // Update is called once per frame
    void Update()
    {
        
    }
}
