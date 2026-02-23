using UnityEngine;

public class End : MonoBehaviour
{
    public void OnClickPlayAgain()
    {
        Debug.Log("restart game button clicked...");
        GameController.Instance.RestartGame(); // restart game
    }

    public void OnClickMainMenu()
    {
        Debug.Log("Main menu button clicked...");
        GameController.Instance.LoadMainMenu(); // load main menu scene
    }

    public void OnClickQuitApplication()
    {
        Debug.Log("Quit application button clicked...");
        GameController.Instance.QuitApplication(); // quit game application
    }
}
