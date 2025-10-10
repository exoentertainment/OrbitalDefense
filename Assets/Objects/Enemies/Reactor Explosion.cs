using System;
using UnityEngine;

public class ReactorExplosion : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private float sizeIncreaseRate;
    [SerializeField] float lightIncreaseRate;
    [SerializeField] private float explosionDuration;
    
    [SerializeField] Light light;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, explosionDuration);
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseLight();
        IncreaseSize();
    }

    void IncreaseSize()
    {
        transform.localScale += new Vector3(sizeIncreaseRate * Time.deltaTime, sizeIncreaseRate * Time.deltaTime, sizeIncreaseRate * Time.deltaTime);
    }

    void IncreaseLight()
    {
        light.intensity += lightIncreaseRate * Time.deltaTime;
    }
}
