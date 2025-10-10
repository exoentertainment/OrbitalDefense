using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region -- Serialized Fields

    [Header("Variables")] 
    [SerializeField] private int musicTrack;
    [SerializeField] private int gameOverDelay;
    
    [SerializeField] private GameObject LoadNextLevelWindow;
    [SerializeField] private GameObject GameOverWindow;

    [Header("GameObjects")]
    [SerializeField] GameObject pauseIcon;
    
    #endregion

    public static GameManager instance;
    
    private bool isLastEnemySet;
    private bool isNextLevelButtonDisplayed;
    bool isGameOver;
    private bool isPaused;
    private int NumPlanets;
    
    private void Start()
    {
        PlayMusic();
        
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
    
        instance = this;
    }

    public void AddPlanet()
    {
        NumPlanets++;
    }

    public void RemovePlanet()
    {
        NumPlanets--;

        if (NumPlanets == 0 && !isGameOver)
        {
            isGameOver = true;
            
            if(AudioManager.instance != null)
                AudioManager.instance.PlayGameOver();
            
            StartCoroutine(LoadGameOverWindow());
        }
    }

    IEnumerator LoadGameOverWindow()
    {
        yield return new WaitForSeconds(gameOverDelay);
        
        GameObject window = (GameObject)Instantiate(Resources.Load("Game Lost"));
    }
    
    void PlayMusic()
    {
        // if(AudioManager.instance != null)
        //     AudioManager.instance.PlayMusic(musicTrack);
    }
    
    public void LoadNextLevelButton()
    {
        StartCoroutine(LoadNextLevelRoutine());
    }

    IEnumerator LoadNextLevelRoutine()
    {
        yield return new WaitForSeconds(5);
        
        LoadNextLevelWindow.SetActive(true);
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlayLevelCleared();
    }
    
    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0.25f;
                isPaused = true;
                pauseIcon.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                isPaused = false;
                pauseIcon.SetActive(false);
            }
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
