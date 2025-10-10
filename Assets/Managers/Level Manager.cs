using System;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject levelOverWindow;
    [SerializeField] private int levelOverDelay;
    
    public static LevelManager instance;

    private bool isBossSpawn;
    
    private bool isLastWave;
    private void Awake()
    {
        if( instance != null && instance != this )
        {
            Destroy(this);
        }
        
        instance = this;
    }

    private void Update()
    {
        CheckLastEnemies();
    }

    public void SetLastWave()
    {
        isLastWave = true;
    }
    
    public void SetBossSpawn()
    {
        isBossSpawn = true;
    }

    void CheckLastEnemies()
    {
        if (isLastWave)
        {
            Collider[] ships = Physics.OverlapSphere(transform.position, Mathf.Infinity, LayerMask.GetMask("Enemy"));;
            
            if (ships.Length == 0)
            {
                isLastWave = false;

                StartCoroutine(EndLevelRoutine());
            }
        }
    }

    IEnumerator EndLevelRoutine()
    {
        yield return new WaitForSeconds(levelOverDelay);
        
        if(levelOverWindow != null)
            levelOverWindow.SetActive(true);
    }
}
