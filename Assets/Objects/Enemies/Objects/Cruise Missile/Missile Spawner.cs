using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MissileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPrefabs;
    [SerializeField] int minSpawnTime;
    [SerializeField] private int maxSpawnTime;
    
    
    SphereCollider sphereCollider;
    float lastSpawnTime;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range((float)minSpawnTime, (float)maxSpawnTime));

            Instantiate(spawnPrefabs[Random.Range(0, spawnPrefabs.Length)], Random.onUnitSphere * sphereCollider.radius,
                Quaternion.identity);
        }
    }
}
