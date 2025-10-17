using UnityEngine;

public class Tutorialevents : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialWindows;

    int currentTutorialWindow = 0;

    public void NextTutorialWindow()
    {
        Destroy(tutorialWindows[currentTutorialWindow]);
        
        currentTutorialWindow++;
        
        if(currentTutorialWindow < tutorialWindows.Length)
            tutorialWindows[currentTutorialWindow].SetActive(true);
    }
}
