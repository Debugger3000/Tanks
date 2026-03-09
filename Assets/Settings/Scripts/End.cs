using UnityEngine;

public class End : MonoBehaviour
{
    public void OnClickPlayAgain()
    {
        GameController.Instance.RestartGame(); // restart game
    }

    public void OnClickMainMenu()
    {
        GameController.Instance.LoadMainMenu(); // load main menu scene
    }

    public void OnClickQuitApplication()
    {
        GameController.Instance.QuitApplication(); // quit game application
    }
}
