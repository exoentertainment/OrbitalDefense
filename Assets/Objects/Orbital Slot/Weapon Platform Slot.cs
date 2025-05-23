using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPlatformSlot : MonoBehaviour, iInteractable
{
    //This holds the installed weapon platform
    private GameObject weaponPlatform;
    
    [SerializeField] GameObject weaponPlatformWindow;
    
    private void Start()
    {
        WeaponPlatformManager.instance.disablePlatformMeshesEvent.AddListener(DisableMeshRenderer);
        WeaponPlatformManager.instance.enablePlatformMeshesEvent.AddListener(EnableMeshRenderer);
    }

    void DisableMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    void EnableMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }
    
    //Take the weapon platform taken from the selection window and instantiate it to weaponPlatform. Then make it a child of this object
    public void PlaceWeaponPlatform(GameObject weaponPlatformObject)
    {
        //If player is replacing an existing platform, destroy the existing one
        if(weaponPlatform != null)
            Destroy(weaponPlatform);
        
        weaponPlatform = Instantiate(weaponPlatformObject, transform.position, Quaternion.Euler(0, 90, 0));
        weaponPlatform.transform.SetParent(transform);

        //gameObject.layer = LayerMask.NameToLayer("Weapon Platform");
    }
    
    //Pause the game and activate the weapon platform selection window. Then pass the gameobject to the selection window
    public void Interact()
    {
        Time.timeScale = 0;
        weaponPlatformWindow.SetActive(true);
        weaponPlatformWindow.GetComponent<WeaponPlatformWindow>().AssignWeaponPlatformSlot(this.gameObject);
    }
}
