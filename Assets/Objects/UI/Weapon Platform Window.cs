using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponPlatformWindow : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    [SerializeField] private PlatformScriptableObject[] weaponPlatformsSO;

    [Header("Components")] 
    private GameObject weaponPlatform;

    enum PlatformType
    {
        Basic = 0,
        DualGun = 1,
        LightMissile = 2,
        CruiseMissile = 3,
        PlasmaGun = 4,
        PlasmaMissile = 5,
        LaserGun = 6,
        StarFort = 7
    }
    
    //Take the passed weapon platform slot and store it if player builds a platform
    public void AssignWeaponPlatformSlot(GameObject slot)
    {
        weaponPlatform = slot;
    }

    //Pass the prefab of the basic platform to the platform slot for instantiation
    public void PlaceBasicWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.Basic].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.Basic].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.Basic].platformPrefab);
            
            if(AudioManager.instance != null)
                AudioManager.instance.PlayUISelect();
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.Basic].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlaceDualGunWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.DualGun].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.DualGun].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlaceLightMissileWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.LightMissile].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.LightMissile].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlaceCruiseMissileWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.CruiseMissile].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.CruiseMissile].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.CruiseMissile].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.CruiseMissile].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlacePlasmaGunWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.PlasmaGun].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.PlasmaGun].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.PlasmaGun].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.PlasmaGun].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlacePlasmaMissileWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.PlasmaMissile].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.PlasmaMissile].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.PlasmaMissile].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.PlasmaMissile].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlaceLaserWeaponPlatform()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.LaserGun].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.LaserGun].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.LaserGun].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.LaserGun].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }
    
    public void PlaceStarFort()
    {
        //check the prefabs scriptable object resource cost against the resource manager
        if (ResourceManager.instance.CheckForResources(weaponPlatformsSO[(int)PlatformType.StarFort].resourceCost))
        {
            if(weaponPlatform.transform.childCount > 0)
                CheckExistingPlatform(weaponPlatformsSO[(int)PlatformType.StarFort].resourceCost/2);
            
            weaponPlatform.GetComponent<WeaponPlatformSlot>()
                .PlaceWeaponPlatform(weaponPlatformsSO[(int)PlatformType.StarFort].platformPrefab);
            
            ResourceManager.instance.DecreaseResources(weaponPlatformsSO[(int)PlatformType.StarFort].resourceCost);
            
            CloseWindow(false);
        }
        else
        {
            AudioManager.instance.PlayUIError();
        }
    }

    void CheckExistingPlatform(int value)
    {
        if(weaponPlatform.transform.childCount > 0)
            ResourceManager.instance.IncreaseResources(value);
    }
    
    //Resume the game and disable the platform selection window
    public void CloseWindow(bool playSound)
    {
        if(playSound && AudioManager.instance != null)
            AudioManager.instance.PlayUIClose();
        
        if(GameManager.instance != null)
            if(!GameManager.instance.IsPaused())
                Time.timeScale = 1;
        
        gameObject.SetActive(false);
    }
}
