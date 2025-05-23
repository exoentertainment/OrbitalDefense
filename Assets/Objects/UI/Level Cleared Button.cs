using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClearedButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            AudioManager.instance.PlayMusic(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
