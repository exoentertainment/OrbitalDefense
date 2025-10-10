using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [Header("Variables")]
    [SerializeField] float timeBetweenWaves;
    [SerializeField] private bool isBossSpawner;
    
    [FormerlySerializedAs("spawnerSO")]
    [Header("Scriptable Object")] 
    [SerializeField] private EnemySpawnerScriptableObject[] spawnWavesSO;
    
    [Header("Component")] 
    [SerializeField] private Transform[] spawnPoint;
    
    #endregion
    
    int currentWave;
    bool isSpawning;

    private void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
        if(!isSpawning)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        if (Time.timeScale > 0)
        {
            isSpawning = true;
            yield return new WaitForSeconds(timeBetweenWaves);

            int numWaves = spawnWavesSO.Length;

            while (numWaves > 0)
            {
                for (int i = 0; i < spawnWavesSO[currentWave].numSpawns; i++)
                {
                    int randomSpawnPoint = Random.Range(0, spawnPoint.Length);
                    int enemySpawn = Random.Range(0, spawnWavesSO[currentWave].enemyShipsPrefabs.Length);
                    
                   Instantiate(spawnWavesSO[currentWave].enemyShipsPrefabs[enemySpawn], spawnPoint[randomSpawnPoint].position,
                            transform.rotation);
                    
                    yield return new WaitForSeconds(Random.Range(spawnWavesSO[currentWave].mintimeBetweenSpawns, spawnWavesSO[currentWave].maxtimeBetweenSpawns));
                }

                numWaves--;
                currentWave++;
                
                yield return new WaitForSeconds(timeBetweenWaves);
            }

            if (isBossSpawner)
            {
                GameObject.FindGameObjectWithTag("Boss incoming").SetActive(true);
            }
            
            LevelManager.instance.SetLastWave();
        }
    }
}