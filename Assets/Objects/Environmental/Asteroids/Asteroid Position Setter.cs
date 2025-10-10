using System;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidPositionSetter : MonoBehaviour
{
    [SerializeField] private int yPositionDeviation;

    private GameObject orbitPoint;
    private MMAutoRotate autoRotate;
    Vector3 startPosition;

    private void Awake()
    {
        autoRotate = GetComponent<MMAutoRotate>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindOrbitPoint();
        SetStartPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FindOrbitPoint()
    {
        autoRotate.OrbitCenterTransform = GameObject.FindGameObjectWithTag("Star").transform;
        autoRotate.enabled = true;
    }
    
    void SetStartPosition()
    {
        startPosition = Random.onUnitSphere * autoRotate.OrbitRadius;
        startPosition.y = Random.Range(-yPositionDeviation, yPositionDeviation);
    }
}
