using System;
using UnityEngine;
using System.Collections;
//using MoreMountains.Feedbacks;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ResourceStation : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --
    
    [Header("Prefabs")]
    [SerializeField] GameObject cargoShipPrefab;
    
    [Header("Components")]
    [SerializeField] Transform cargoShipSpawnPoint;

    [Header("Scriptable Object")] 
    [SerializeField] private ResourceStationScriptableObject resourceStationSO;

    [SerializeField] private Transform[] explosionPoints;
    
    [Header("Events")]
    [SerializeField] UnityEvent OnDeath;
    
    [Header("Feedbacks")]
    //[SerializeField] MMFeedbacks deathFeedback;
    
    #endregion

    private float lastSpawnTime;
    private float currentHealth;
    bool isDestroyed;
    private bool isBeingHit;

    private void Start()
    {
        lastSpawnTime = Time.time;
        currentHealth = resourceStationSO.maxHealth;
    }

    private void Update()
    {
        SpawnCargoShip();
    }

    void SpawnCargoShip()
    {
        if ((Time.time - lastSpawnTime) >= resourceStationSO.cargoShipSpawnTime)
        {
            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, resourceStationSO.resourceStationLayerMask);

            if (potentialTargets.Length > 1)
            {
                GameObject cargoShip =
                    Instantiate(cargoShipPrefab, cargoShipSpawnPoint.position, Quaternion.identity);
                cargoShip.GetComponent<CargoShipMovement>().SetOriginStation(gameObject);

                lastSpawnTime = Time.time;
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (!isBeingHit)
        {
            isBeingHit = true;
            currentHealth -= damage;
        }

        if (currentHealth <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            OnDeath?.Invoke();
            
            StartCoroutine(DestroyPlatformRoutine());
        }
    }
    
    IEnumerator DestroyPlatformRoutine()
    {
        for (int i = 0; i < resourceStationSO.numExplosions; i++)
        {
            Instantiate(resourceStationSO.explosionPrefab, explosionPoints[Random.Range(0, explosionPoints.Length)].position, Quaternion.identity);
            
            if(AudioManager.instance != null)
                AudioManager.instance.PlaySound(resourceStationSO.explosionSFX);
                
            //deathFeedback?.PlayFeedbacks();
            
            yield return new WaitForSeconds(resourceStationSO.explosionFrequency);
        }
        
        Destroy(gameObject);
    }
}
