using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void onPlayButton()
    {
        SceneManager.LoadScene("SafeArea");
    }

    public void onQuitButton()
    {
        Application.Quit();
    }
}
