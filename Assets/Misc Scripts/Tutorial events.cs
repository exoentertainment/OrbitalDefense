using System;
using UnityEngine;

public class Tutorialevents : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialWindows;

    int currentTutorialWindow = 0;

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void NextTutorialWindow()
    {
        if(AudioManager.instance != null)
            AudioManager.instance.PlayUISelect();
        
        Destroy(tutorialWindows[currentTutorialWindow]);
        
        currentTutorialWindow++;
        
        if(currentTutorialWindow < tutorialWindows.Length)
            tutorialWindows[currentTutorialWindow].SetActive(true);
    }
}
