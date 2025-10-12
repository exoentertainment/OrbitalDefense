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
    private bool isBossLevel;

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
                waveText.text = "Wave " + (currentWave + 1) + " of " + numWaves;
                lastSpawnTime = Time.time;
            }
            else
            {
                bossSpawner?.Invoke();
                
                if(bossSpawner == null)
                {
                    LevelManager.instance.SetLastWave();
                }
            }
            
            currentWave++;
        }
        
        //StartCoroutine(SpawnEnemyRoutine());
    }
}
