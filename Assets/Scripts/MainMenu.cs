using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ButtonStart()
    {
        SceneManager.LoadScene(1);

        if (!AudioManager.Instance.InitializeAudio())
        {
            Debug.LogError("Failed to initialise Audio");
        }
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
