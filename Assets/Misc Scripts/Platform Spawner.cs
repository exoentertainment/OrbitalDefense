using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] weaponPlatforms;
    [SerializeField] PlatformScriptableObject[] weaponPlatformsSO;

    private void Start()
    {
        foreach (GameObject platform in weaponPlatforms)
        {
            platform.GetComponent<WeaponPlatformSlot>().PlaceWeaponPlatform(weaponPlatformsSO[Random.Range(0, weaponPlatformsSO.Length)].platformPrefab);
        }
    }
}
