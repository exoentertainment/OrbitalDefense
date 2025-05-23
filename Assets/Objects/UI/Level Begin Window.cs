using UnityEngine;

public class LevelBeginWindow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0;
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
