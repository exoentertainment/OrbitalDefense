using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void OpenSettings()
    {
        GameObject audioSettings = (GameObject)Instantiate(Resources.Load("Audio Settings"));
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    public void LoadControlsWindow()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Controls Window"));
    }
    
    public void QuitGame()
    {
        SceneManager.LoadScene("Start Menu");
    }
}
