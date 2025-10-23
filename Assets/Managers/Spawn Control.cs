using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using VHierarchy.Libs;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] private int numWaves;
    [SerializeField] private float timeBetweenWaves;

    [SerializeField] private UnityEvent enemySpawners;
    [SerializeField] private UnityEvent bossSpawner;
    
    [SerializeField] TMPro.TMP_Text waveText;
    
    private int currentWave;
    bool isSpawning;
    float lastSpawnTime;
    private bool isBossSpawned;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        //if(!isSpawning)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        isSpawning = true;
        
        if ((Time.time - lastSpawnTime) >= timeBetweenWaves)
        {
            if (currentWave < numWaves)
            {
                enemySpawners?.Invoke();
                
                if(waveText != null)
                    waveText.text = "Wave " + (currentWave + 1) + " of " + numWaves;
                
                lastSpawnTime = Time.time;
                currentWave++;

                if (currentWave >= numWaves && bossSpawner.GetPersistentEventCount() == 0)
                {
                    if(LevelManager.instance != null)
                        LevelManager.instance.SetLastWave();
                }
            }
            else
            {
                if (!isBossSpawned && bossSpawner.GetPersistentEventCount() > 0) 
                {
                    isBossSpawned = true;
                    bossSpawner?.Invoke();
                    LevelManager.instance.SetLastWave();
                }
            }
        }
        
        //StartCoroutine(SpawnEnemyRoutine());
    }
}
