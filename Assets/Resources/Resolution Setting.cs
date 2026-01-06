using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSetting : MonoBehaviour
{
    [SerializeField] private Sprite selectedButton;
    [SerializeField] private Sprite unselectedButton;
    [SerializeField] Image buttonImage;
    [SerializeField] TMP_Text resolutionText;
    [SerializeField] TMP_Text currentResolutionText;
    
    Resolution[] availableResolution;
    int currentResolution;
    private bool isWindowed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableResolution = Screen.resolutions;
        ShowCurrentResolution();
        ChangeResolutionText();
    }

    public void SetWindowed()
    {
        isWindowed = !isWindowed;
        
        if(isWindowed)
            buttonImage.sprite = selectedButton;
        else
            buttonImage.sprite = unselectedButton;
    }

    void ShowCurrentResolution()
    {
        currentResolutionText.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
    }
    
    public void NextResolution()
    {
        currentResolution++;
        
        if(currentResolution >= availableResolution.Length)
            currentResolution = 0;
        
        ChangeResolutionText();
    }

    public void PreviousResolution()
    {
        currentResolution--;
        
        if(currentResolution < 0)
            currentResolution = availableResolution.Length - 1;
        
        ChangeResolutionText();
    }

    void ChangeResolutionText()
    {
        resolutionText.text = availableResolution[currentResolution].width + " x " + availableResolution[currentResolution].height;
    }

    public void ApplySettings()
    {
        Screen.SetResolution(availableResolution[currentResolution].width, availableResolution[currentResolution].height, !isWindowed);
    }

    public void CloseWindow()
    {
        //GameObject window = (GameObject)Instantiate(Resources.Load("Pause Menu"));
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
