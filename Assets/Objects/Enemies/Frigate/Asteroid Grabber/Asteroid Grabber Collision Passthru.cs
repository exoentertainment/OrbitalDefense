using System;
using UnityEngine;

public class AsteroidGrabberCollisionPassthru : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("collide");
    }
}
