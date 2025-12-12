using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MissileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPrefabs;
    [FormerlySerializedAs("minSpawnTime")] [SerializeField] int spawnTime;
    
    SphereCollider sphereCollider;
    
    float lastSpawnTime;
    private bool isSpawning;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if(isSpawning)
            SpawnProjectile();
    }

    void SpawnProjectile()
    {
        if ((Time.time - lastSpawnTime) > spawnTime)
        {
            Instantiate(spawnPrefabs[0], Random.onUnitSphere * sphereCollider.radius,  Quaternion.identity);
            lastSpawnTime = Time.time;
        }
    }
    
    public void ActivateSpawner()
    {
        isSpawning = true;
    }
}
