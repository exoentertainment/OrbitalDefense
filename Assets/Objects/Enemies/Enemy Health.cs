using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable, IRepairable
{
    #region --Serialized Fields--

    [SerializeField] private EnemySO enemySO;
    [SerializeField] Slider healthSlider;
    [SerializeField] private UnityEvent onDeath;
    [SerializeField] MMFeedbacks deathFeedback;

    [SerializeField] private Transform[] explosionPoints;
    
    #endregion

    private float maxHealth;
    float currentHealth;
    bool isDead;

    private bool isBeingHit;

    private void Start()
    {
        currentHealth = Random.Range(enemySO.minHealth, enemySO.maxHealth + 1);
        maxHealth = currentHealth;
    }

    private void Update()
    {
        if(isBeingHit)
            isBeingHit = false;
        
        healthSlider.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(float damage)
    {
        if (!isBeingHit && currentHealth > 0)
        {
            currentHealth -= damage;
            UpdateHealthBar();
            isBeingHit = true;
        }

        if(currentHealth <= 0 && !isDead)
            OnDeath();
    }

    void OnDeath()
    {
        isDead = true;
        
        if(currentHealth <= 0)
            ResourceManager.instance.IncreaseResources(enemySO.pointValue);
        
        //Invoke onDeath event
        deathFeedback?.PlayFeedbacks();
        onDeath?.Invoke();
        
        //start coroutine that spawns explosions along ship
        StartCoroutine(SpawnExplosionsRoutine());
    }

    IEnumerator SpawnExplosionsRoutine()
    {
        for (int i = 0; i < enemySO.numExplosions; i++)
        {
            Instantiate(enemySO.explosionPrefab, explosionPoints[Random.Range(0, explosionPoints.Length)].position, Quaternion.identity);
                
            // if(AudioManager.instance != null)
            //     AudioManager.instance.PlaySound(enemySO.shipExplosion);
            
            // if(CameraManager.instance.IsObjectInView(gameObject.transform))
            //     AudioManager.instance.PlayPlatformExplosion();
            
            yield return new WaitForSeconds(enemySO.explosionFrequency);
        }
        
        Destroy(gameObject);
    }

    IEnumerator SpawnReactorExplosion()
    {
        yield return new WaitForSeconds(enemySO.explosionDuration * .75f);
        
        Instantiate(enemySO.reactorExplosionPrefab, transform.position, Quaternion.identity);
    }

    public void TriggerDeath()
    {
        OnDeath();
    }

    public float GetHealth()
    {
        return currentHealth / enemySO.maxHealth;
    }

    public void RepairHealth(int value)
    {
        currentHealth += value;
        UpdateHealthBar();
        
        if(currentHealth > enemySO.maxHealth)
            currentHealth = enemySO.maxHealth;
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/maxHealth;
    }
}
