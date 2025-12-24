using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnLightPrefabs;
    [SerializeField] private GameObject[] spawnFrigatePrefabs;
    [SerializeField] private Transform[] spawnLightPoints;
    [SerializeField] private Transform spawnFrigatePoint;
    [SerializeField] int spawnDelay;
    
    float lastSpawnTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnLightEnemy();
        SpawnFrigateEnemy();
    }

    private void SpawnLightEnemy()
    {
        if (Time.time - lastSpawnTime > spawnDelay)
        {
            foreach (Transform spawn in spawnLightPoints)
            {
                Instantiate(spawnLightPrefabs[Random.Range(0, spawnLightPrefabs.Length)], spawn.position, spawn.rotation);
            }
        }
    }
    
    private void SpawnFrigateEnemy()
    {
        if (Time.time - lastSpawnTime > spawnDelay)
        {
            Instantiate(spawnFrigatePrefabs[Random.Range(0, spawnFrigatePrefabs.Length)], spawnFrigatePoint.position, spawnFrigatePoint.rotation);
            lastSpawnTime = Time.time;
        }
    }
}
