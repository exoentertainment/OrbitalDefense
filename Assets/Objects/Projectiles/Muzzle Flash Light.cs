using System;
using UnityEngine;

public class MuzzleFlashLight : MonoBehaviour
{
    [SerializeField] private float flashDuration;
    
    Light light;
    
    private void Awake()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        DisableLight();
    }

    void DisableLight()
    {
        flashDuration -= Time.deltaTime;
        
        if(flashDuration <= 0)
            light.enabled = false;
    }
}
