using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMissileLauncher : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] MissileLauncherSO missileLauncherSO;
    [SerializeField] private Transform[] spawnPoints;

    #endregion
    
    float lastFireTime;
    private bool isFiring = true;
    
    
    ObjectPool projectilePool;

    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }
    
    public void ActivateGun()
    {
        isFiring = true;
    }

    public void DeactivateGun()
    {
        isFiring = false;
    }
    
    void Fire()
    {
        if(isFiring)
            if ((Time.time - lastFireTime) > missileLauncherSO.fireRate)
            {
                lastFireTime = Time.time;
                    SetSingleTarget();
            }
    }

    //This is called if current missile selects closest target. Culls any nearby target that isn't on screen
    void SetSingleTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileLauncherSO.projectileSO.range, missileLauncherSO.projectileSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            if (possibleTargets.Length > 0)
            {
                GameObject target = null;
                float closestEnemy = Mathf.Infinity;

                for (int x = 0; x < possibleTargets.Length; x++)
                {
                    float distanceToEnemy =
                        Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;
                        target = possibleTargets[x].gameObject;
                    }
                }
                
                foreach (Transform spawnPoint in spawnPoints)
                {
                    GameObject missile = Instantiate(missileLauncherSO.projectileSO.projectilePrefab, spawnPoint.position, transform.rotation);
                    missile.GetComponent<LightMissile>().SetTarget(target);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, missileLauncherSO.projectileSO.range);
    }
}
