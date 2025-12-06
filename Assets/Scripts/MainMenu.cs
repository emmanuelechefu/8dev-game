using UnityEngine;
using UnityEngine.SceneManagement;

//Icl idk how to make a main menu LOOLL goodluck to future devs

public class MainMenu : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("SafeArea");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
