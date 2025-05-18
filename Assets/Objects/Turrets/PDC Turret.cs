using System;
using System.Collections;
using System.Numerics;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PDCTurret : MonoBehaviour
{
    #region --Serialized Fields

    [SerializeField] PDCSO platformSO;
    [SerializeField] private MMAutoRotate[] barrels;

    [SerializeField] private Transform platformBase;
    [SerializeField] private Transform platformTurret;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform raycastOrigin;
    
    [SerializeField] private bool doesRotate;

    #endregion

    private GameObject target;
    private float lastFireTime;
    ObjectPool projectilePool;
    
    float yCurrentRotation;

    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }
    
    private void Start()
    {
        lastFireTime = Time.time;
    }

    private void Update()
    {
        Debug.Log(platformBase.rotation.eulerAngles.y);
        if (target != null && target.activeSelf)
        {
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * platformSO.projectileSO.range, Color.red);
            RotateTowardsTarget();
            Fire();
            CheckDistanceToTarget();
        }
        else
        {
            SearchForTarget();
            
            foreach (MMAutoRotate barrel in barrels)
            {
                barrel.enabled = false;
            }
        }

        
    }

    //Find the closest target going in order of target priority. If a suitable target cant be found in the first priority then check for targets in the next priority
    void SearchForTarget()
    {
        if (target == null || !target.activeSelf)
        {
            Collider[] possibleTargets = Physics.OverlapSphere(transform.position, platformSO.projectileSO.range,
                platformSO.targetMask);
            
            if (possibleTargets.Length > 0)
            {
                float closestEnemy = platformSO.projectileSO.range;
    
                for (int x = 0; x < possibleTargets.Length; x++)
                {
                    float distanceToEnemy =
                        Vector3.Distance(possibleTargets[x].transform.position, transform.position);
    
                    if (IsLoSClear(possibleTargets[x].gameObject))
                        if (distanceToEnemy < closestEnemy && distanceToEnemy > platformSO.minRange)
                        {
                            closestEnemy = distanceToEnemy;
                            target = possibleTargets[x].gameObject;
                        }
                }
    
                // if (target != null)
                //     break;
            }
        }
    }
    
    //Check if the passed target is within line-of-sight. If it is, then return true
    bool IsLoSClear(GameObject obj)
    {
        if (Physics.Raycast(raycastOrigin.position, obj.transform.position - raycastOrigin.position, out RaycastHit hit, platformSO.projectileSO.range, platformSO.targetMask))
        {
            if (hit.collider != null)
            {
                // if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Projectile"))
                // {
                //     lastTimeOnTarget = Time.time;
                //     return true;
                // }
                
                return true;
            }
        }
        
        return false;
    }

    //Rotate the base and barrel towards target
    void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);
            targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
            float baseYRotation = targetRotation.eulerAngles.y;

            platformBase.rotation = Quaternion.SlerpUnclamped(platformBase.rotation, targetRotation, platformSO.baseTrackingSpeed * Time.deltaTime);
            
            targetVector = target.transform.position - platformTurret.transform.position;
            targetVector.Normalize();
            targetRotation = Quaternion.LookRotation(targetVector);
            
            if ((baseYRotation - platformBase.rotation.eulerAngles.y) < 20f)
                platformTurret.rotation = Quaternion.SlerpUnclamped(platformTurret.rotation, targetRotation, platformSO.barrelTrackingSpeed * Time.deltaTime);
        }
    }

    void Fire()
    {
        foreach (MMAutoRotate barrel in barrels)
        {
            if(!barrel.enabled)
                barrel.enabled = true;
        }
        
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        if ((Time.time - lastFireTime) > platformSO.RoF)
        {
            lastFireTime = Time.time;
            Quaternion shootingAngle = new Quaternion();
            shootingAngle.eulerAngles = new Vector3(platformTurret.rotation.eulerAngles.x + platformSO.GetTrackingError(), platformTurret.rotation.eulerAngles.y + platformSO.GetTrackingError(), 0);
            
            // if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * turretSO.projectileSO.range, out RaycastHit hit, turretSO.projectileSO.range))
            //     if(hit.collider.gameObject.CompareTag("Enemy"))
                    foreach (Transform spawnPoint in spawnPoints)
                    {
                        GameObject projectile = projectilePool.GetPooledObject(); 
                        if (projectile != null) {
                            projectile.transform.position = spawnPoint.position;
                            projectile.transform.rotation = shootingAngle;
                            projectile.SetActive(true);
                        }

                        yield return new WaitForSeconds(platformSO.barrelFireDelay);
                    }
            
            //AudioManager.instance.PlaySound(turretSO.fireSFX);
        }
    }

    void CheckDistanceToTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < platformSO.minRange)
            target = null;
    }

    //Disable this script. Called by the weapon manager
    public void TurnActivityOff()
    {
        this.enabled = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, platformSO.projectileSO.range);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, platformSO.minRange);
    }
}
