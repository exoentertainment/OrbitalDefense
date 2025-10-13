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
    [SerializeField] GameObject bossPrefab;
    
    [FormerlySerializedAs("spawnerSO")]
    [Header("Scriptable Object")] 
    [SerializeField] private EnemySpawnerScriptableObject[] spawnWavesSO;
    
    #endregion
    
    int currentWave;

    public void SpawnEnemy()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        if (Time.timeScale > 0)
        {
            // isSpawning = true;
            // yield return new WaitForSeconds(timeBetweenWaves);
            //
            // int numWaves = spawnWavesSO.Length;
            //
            // while (numWaves > 0)
            // {
            //     for (int i = 0; i < spawnWavesSO[currentWave].numSpawns; i++)
            //     {
            //         int randomSpawnPoint = Random.Range(0, spawnPoint.Length);
            //         int enemySpawn = Random.Range(0, spawnWavesSO[currentWave].enemyShipsPrefabs.Length);
            //         
            //        Instantiate(spawnWavesSO[currentWave].enemyShipsPrefabs[enemySpawn], spawnPoint[randomSpawnPoint].position,
            //                 transform.rotation);
            //         
            //         yield return new WaitForSeconds(Random.Range(spawnWavesSO[currentWave].mintimeBetweenSpawns, spawnWavesSO[currentWave].maxtimeBetweenSpawns));
            //     }
            //
            //     numWaves--;
            //     currentWave++;
            //     
            //     yield return new WaitForSeconds(timeBetweenWaves);
            // }
            //
            // if (isBossSpawner)
            // {
            //     GameObject.FindGameObjectWithTag("Boss incoming").SetActive(true);
            // }
            //
            // LevelManager.instance.SetLastWave();

            if(currentWave >= spawnWavesSO.Length)
                yield break;
            
            for (int x = 0; x < spawnWavesSO[currentWave].numSpawns; x++)
            {
                int enemySpawn = Random.Range(0, spawnWavesSO[currentWave].enemyShipsPrefabs.Length);
                Instantiate(spawnWavesSO[currentWave].enemyShipsPrefabs[enemySpawn], transform.position,
                         transform.rotation);
                
                yield return new WaitForSeconds(Random.Range(spawnWavesSO[currentWave].mintimeBetweenSpawns, spawnWavesSO[currentWave].maxtimeBetweenSpawns));
            }

            currentWave++;
        }
    }
    
    public void SpawnBoss()
    {   
        GameObject.FindGameObjectWithTag("Boss incoming").SetActive(true);
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
    }
}