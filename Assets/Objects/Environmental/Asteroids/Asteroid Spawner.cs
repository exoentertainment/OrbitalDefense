using System;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] Transform orbitPoint;
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private int numAsteroids;

    [SerializeField] float orbitSpeed;
    [SerializeField] float orbitRange;

    private void Start()
    {
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        for (int i = 0; i < numAsteroids; i++)
        {
            GameObject asteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                orbitPoint.position + new Vector3(Random.Range(-orbitRange, orbitRange), 0, Random.Range(-orbitRange, orbitRange)),
                Quaternion.identity);
            asteroid.GetComponent<MMAutoRotate>().OrbitRadius = orbitRange;
            asteroid.GetComponent<MMAutoRotate>().OrbitRotationSpeed = orbitSpeed;
            asteroid.GetComponent<MMAutoRotate>().OrbitCenterTransform = orbitPoint;
            asteroid.GetComponent<MMAutoRotate>().OrbitCenterOffset.y = Random.Range(-200, 200);
            asteroid.transform.SetParent(transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(orbitPoint.position, orbitRange);
    }
}
