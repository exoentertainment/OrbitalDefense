using System;
using UnityEngine;
using UnityEngine.Events;

public class CargoShipHealth : MonoBehaviour, IDamageable
{
    [SerializeField] CargoShipScriptableObject cargoShipSO;
    
    float currentHealth;
    bool isDead;

    private bool isBeingHit;

    private void Start()
    {
        currentHealth = cargoShipSO.maxHealth;
    }
    
    private void Update()
    {
        if(isBeingHit)
            isBeingHit = false;
    }
    
    public void TakeDamage(float damage)
    {
        if (!isBeingHit && currentHealth > 0)
        {
            currentHealth -= damage;
            isBeingHit = true;
        }

        if(currentHealth <= 0 && !isDead)
            OnDeath();
    }
    
    void OnDeath()
    {
        isDead = true;
        
        SpawnExplosion();
        Destroy(gameObject);
    }

    void SpawnExplosion()
    {
        Instantiate(cargoShipSO.explodePrefab, transform.position, Quaternion.identity);
    }
}
