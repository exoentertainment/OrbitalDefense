using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Image pauseHighlight;
    [SerializeField] Image oneSpeedHighlight;
    [SerializeField] Image twoSpeedHighlight;
    [SerializeField] Image threeSpeedHighlight;
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        
        pauseHighlight.gameObject.SetActive(true);
        oneSpeedHighlight.gameObject.SetActive(false);
        twoSpeedHighlight.gameObject.SetActive(false);
        threeSpeedHighlight.gameObject.SetActive(false);
    }

    public void SetNormalSpeed()
    {
        Time.timeScale = 1;
        
        pauseHighlight.gameObject.SetActive(false);
        oneSpeedHighlight.gameObject.SetActive(true);
        twoSpeedHighlight.gameObject.SetActive(false);
        threeSpeedHighlight.gameObject.SetActive(false);
    }

    public void SetTwoSpeed()
    {
        Time.timeScale = 2;
        
        pauseHighlight.gameObject.SetActive(false);
        oneSpeedHighlight.gameObject.SetActive(false);
        twoSpeedHighlight.gameObject.SetActive(true);
        threeSpeedHighlight.gameObject.SetActive(false);
    }

    public void SetThreeSpeed()
    {
        Time.timeScale = 3;
        
        pauseHighlight.gameObject.SetActive(false);
        oneSpeedHighlight.gameObject.SetActive(false);
        twoSpeedHighlight.gameObject.SetActive(false);
        threeSpeedHighlight.gameObject.SetActive(true);
    }
}
