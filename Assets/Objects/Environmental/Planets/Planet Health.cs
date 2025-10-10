using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlanetHealth : MonoBehaviour, IDamageable
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")]
    [SerializeField] PlanetHealthSO planetHealthSO;

    [Header("Components")]
    [SerializeField] Slider healthSlider;
    
    [Header("Materials")]
    [SerializeField] Material deadMaterial;

    #endregion

    private float currentHealth;
    SphereCollider sphereCollider;
    private bool isBeingHit;
    bool isDead;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        currentHealth = planetHealthSO.maxHealth;
        GameManager.instance.AddPlanet();
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

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            DestroyPlanet();
        }
    }
    
    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth/planetHealthSO.maxHealth;
    }

    void DestroyPlanet()
    {
        StartCoroutine(DestroyPlanetCoroutine());
        gameObject.layer = LayerMask.NameToLayer("Destroyed Planet");

    }

    IEnumerator DestroyPlanetCoroutine()
    {
        float timer = 0f;

        while (timer < planetHealthSO.explosionDuration)
        {
            timer += planetHealthSO.delayPerExplosion;
            
            Vector3 pos = transform.position + Random.onUnitSphere * (sphereCollider.radius * transform.lossyScale.x);
            
            Instantiate(planetHealthSO.explosionPrefab, pos, Quaternion.identity);

            yield return new WaitForSeconds(planetHealthSO.delayPerExplosion);
        }
        
        GetComponent<MeshRenderer>().material = deadMaterial;
        GameManager.instance.RemovePlanet();
    }
}
