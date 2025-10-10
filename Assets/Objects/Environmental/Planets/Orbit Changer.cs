using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class OrbitChanger : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private int orbitChangeFrequency;
    [SerializeField] private int orbitChangeDuration;
    
    [SerializeField] private float orbitRadiusVariation;
    [SerializeField] private int minOrbitRadius;
    [SerializeField] private int maxOrbitRadius;
    
    [SerializeField] float orbitSpeedVariation;
    [SerializeField] int minOrbitSpeed;
    [SerializeField]  int maxOrbitSpeed;
    
    [SerializeField] float orbitAxisVariation;
    
    [SerializeField] float orbitOffsetVariation;
    [SerializeField] int minOrbitOffset;
    [SerializeField] int maxOrbitOffset;

    [SerializeField] private GameObject window;
    [SerializeField] int windowOpenTime;
    
    [SerializeField] MMAutoRotate[] planets;
    [SerializeField] UnityEvent onPlanetOrbitChange;

    #endregion


    private float lastOrbitChange;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastOrbitChange = Time.time;
    }

    private void Update()
    {
        CheckPlanetOrbitTime();
    }

    //Check to see if enough time has passed to change orbits
    void CheckPlanetOrbitTime()
    {
        if(Time.time - lastOrbitChange >= orbitChangeFrequency)
        {
            onPlanetOrbitChange?.Invoke();
            StartPlanetOrbitRoutine();
            lastOrbitChange = Time.time;
        }
    }
    
    //Go through each planet and begin changing its orbit parameters
    void StartPlanetOrbitRoutine()
    {
        for(int i = 0; i < planets.Length; i++)
        {
            StartCoroutine(ChangePlanetOrbitRadiusRoutine(i));

            StartCoroutine(ChangePlanetOrbitSpeedRoutine(i));

            StartCoroutine(ChangePlanetOrbitOffsetRoutine(i));
        }
    }

    //Change the planets orbit radius incrementally for the change duration 
    IEnumerator ChangePlanetOrbitRadiusRoutine(int planetIndex)
    {
        float changeAmount = Random.Range(-(planets[planetIndex].OrbitRadius * orbitRadiusVariation),
            (planets[planetIndex].OrbitRadius * orbitRadiusVariation));
        
        float currentTime = 0;

        while (currentTime < orbitChangeDuration)
        {
            currentTime += Time.deltaTime;
            
            if((planets[planetIndex].OrbitRadius + changeAmount * Time.deltaTime) >= minOrbitRadius && (planets[planetIndex].OrbitRadius + changeAmount * Time.deltaTime) <= maxOrbitRadius )
                planets[planetIndex].OrbitRadius += changeAmount * Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    //Change the planets orbit speed incrementally for the change duration 
    IEnumerator ChangePlanetOrbitSpeedRoutine(int planetIndex)
    {
        float changeAmount = Random.Range(-(planets[planetIndex].OrbitRotationSpeed * orbitSpeedVariation),
            (planets[planetIndex].OrbitRotationSpeed * orbitSpeedVariation));
        float currentTime = 0;

        while (currentTime < orbitChangeDuration)
        {
            currentTime += Time.deltaTime;
            
            if((planets[planetIndex].OrbitRotationSpeed + changeAmount * Time.deltaTime) >= minOrbitSpeed && (planets[planetIndex].OrbitRotationSpeed + changeAmount * Time.deltaTime) <= maxOrbitSpeed)
                planets[planetIndex].OrbitRotationSpeed += changeAmount * Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    //Change the planets orbit axis incrementally for the change duration 
    IEnumerator ChangePlanetOrbitAxisRoutine(int planetIndex)
    {
        float changeAmount = Random.Range(-180, 180);
        float currentTime = 0;

        while (currentTime < orbitChangeDuration)
        {
            currentTime += Time.deltaTime;
            
            //if((planets[planetIndex].OrbitRotationAxis.x + changeAmount * Time.deltaTime) >= minOrbitSpeed && (planets[planetIndex].OrbitRotationAxis.x + changeAmount * Time.deltaTime) <= maxOrbitSpeed)
                planets[planetIndex].OrbitRotationAxis.x += changeAmount * Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    //Change the planets orbit speed incrementally for the change duration 
    IEnumerator ChangePlanetOrbitOffsetRoutine(int planetIndex)
    {
        // float changeAmount = Random.Range(-(planets[planetIndex].OrbitCenterOffset.x * orbitOffsetVariation),
        //     (planets[planetIndex].OrbitCenterOffset.x * orbitOffsetVariation));

        float changeAmount;
            
        if(planets[planetIndex].OrbitCenterOffset.x > -100 && planets[planetIndex].OrbitCenterOffset.x < 100)
            changeAmount = Random.Range((-100 * orbitOffsetVariation), (100 * orbitOffsetVariation));
        else
            changeAmount = Random.Range(-(planets[planetIndex].OrbitCenterOffset.x * orbitOffsetVariation),
                (planets[planetIndex].OrbitCenterOffset.x * orbitOffsetVariation));
        
        float currentTime = 0;

        while (currentTime < orbitChangeDuration)
        {
            currentTime += Time.deltaTime;
            
            if((planets[planetIndex].OrbitCenterOffset.x + changeAmount * Time.deltaTime) >= minOrbitOffset && (planets[planetIndex].OrbitCenterOffset.x + changeAmount * Time.deltaTime) <= maxOrbitOffset)
                planets[planetIndex].OrbitCenterOffset.x += changeAmount * Time.deltaTime;
            else
                planets[planetIndex].OrbitCenterOffset.x -= changeAmount * Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
    }
}
