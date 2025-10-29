using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndlessMode()
    {
        
    }

    public void Settings()
    {
        GameObject window = (GameObject)Instantiate(Resources.Load("Settings Menu"));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
