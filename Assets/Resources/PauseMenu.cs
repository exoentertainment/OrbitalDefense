using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void OpenAudioSettings()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Audio Settings"));
        Destroy(gameObject);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    public void LoadControlsWindow()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Controls Window"));
        Destroy(gameObject);
    }

    public void LoadResolutionWindow()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Resolution Window"));
        Destroy(gameObject);
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene("Start Menu");
    }
}
