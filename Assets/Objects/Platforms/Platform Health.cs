using System;
using System.Collections;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlatformHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] PlatformScriptableObject healthSO;
    
    [Header("Components")]
    [SerializeField] Slider healthSlider;
    [SerializeField] private Transform[] explosionPoints;
    
    [Header("Events")]
    [SerializeField] UnityEvent OnDeath;
    
    [Header("Feedbacks")]
    [SerializeField] MMFeedbacks deathFeedback;

    #endregion
    
    float currentHealth; 
    bool isDestroyed;
    private bool isBeingHit;

    private void Start()
    {
        currentHealth = healthSO.maxHealth;
    }

    private void Update()
    {
        healthSlider.transform.LookAt(Camera.main.transform);
        
        if(isBeingHit)
            isBeingHit = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isBeingHit)
        {
            isBeingHit = true;
            currentHealth -= damage;
            UpdateHealthBar();
        }

        if (currentHealth <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            OnDeath?.Invoke();
            
            // if(CameraManager.instance.IsObjectInView(gameObject.transform))
            //     AudioManager.instance.PlayPlatformExplosion();
                
            //deathFeedback?.PlayFeedbacks();
            //Instantiate(healthSO.explosionPrefab, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            
            StartCoroutine(DestroyPlatformRoutine());
        }
    }

    IEnumerator DestroyPlatformRoutine()
    {
        for (int i = 0; i < healthSO.numExplosions; i++)
        {
            Instantiate(healthSO.explosionPrefab, explosionPoints[Random.Range(0, explosionPoints.Length)].position, Quaternion.identity);
            
            if(AudioManager.instance != null)
                AudioManager.instance.PlaySound(healthSO.explosionSFX);
                
            deathFeedback?.PlayFeedbacks();
            
            yield return new WaitForSeconds(healthSO.explosionFrequency);
        }
        
        Destroy(gameObject);
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/healthSO.maxHealth;
    }
}
