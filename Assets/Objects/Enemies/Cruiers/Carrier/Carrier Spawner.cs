using UnityEngine;

public class CarrierSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPrefabs;
    [SerializeField] private Transform spawnPoint;
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
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (Time.time - lastSpawnTime > spawnDelay)
        {
                Instantiate(spawnPrefabs[Random.Range(0, spawnPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
