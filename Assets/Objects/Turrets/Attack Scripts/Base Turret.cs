using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(ObjectPool))]
public class BaseTurret : MonoBehaviour
{
    #region --Serialized Fields
    
    [FormerlySerializedAs("turretSO")] [SerializeField] protected TurretSO turretSO;
    
    [SerializeField] protected  Transform platformTurret;
    [SerializeField] protected Transform[] spawnPoints;
    [SerializeField] protected Transform raycastOrigin;

    #endregion

    protected GameObject target;

    protected float lastFireTime;
    protected float lastTimeOnTarget;
    
    Vector2 currentRotation;
    private float currentAngle;
    private bool targetLOS;
    
    protected ObjectPool projectilePool;

    protected void Awake()
    {
        projectilePool = GetComponent<ObjectPool>(); ;
    }

    protected void Update()
    {
        if (target != null)
        {
            Debug.DrawLine(raycastOrigin.position, raycastOrigin.position + (raycastOrigin.transform.forward * turretSO.projectileSO.range), Color.red);
            Fire();
        }
    }

    //Find the closest target going in order of target priority. If a suitable target cant be found in the first priority then check for targets in the next priority
    protected virtual void SearchForTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, turretSO.projectileSO.range, turretSO.projectileSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                //if (IsLoSClear(possibleTargets[x].gameObject))
                    if (distanceToEnemy < closestEnemy)
                    {
                            closestEnemy = distanceToEnemy;
                            target = possibleTargets[x].transform.root.gameObject;
                    }
            }
        }

        if (target != null)
        {
            lastFireTime = Time.time;
            lastTimeOnTarget = Time.time;
        }
    }
    
    //Check if the passed target is within line-of-sight. If it is, then return true
    protected virtual bool IsLoSClear()
    {
        //Standard raycast
        if(Physics.Raycast(raycastOrigin.position, target.transform.position - raycastOrigin.position, out RaycastHit hit))
        {
            if (hit.collider.transform.root.gameObject == target.transform.root.gameObject)
            {
                lastTimeOnTarget = Time.time;
                return true;
            }
        }
        
        return false;
    }
    // protected virtual void IsLoSClear()
    // {
    //     // if(Physics.Linecast(raycastOrigin.position, target.transform.position, out RaycastHit hit))
    //     if(Physics.Linecast(raycastOrigin.position, raycastOrigin.position + (raycastOrigin.transform.forward * turretSO.projectileSO.range), out RaycastHit hit))
    //     {
    //         if (hit.collider.gameObject == target)
    //         {
    //             lastTimeOnTarget = Time.time;
    //         }
    //     }
    // }
    
    protected void Fire()
    {
        if ((Time.time - lastFireTime) > turretSO.fireRate)
        {
            if(IsLoSClear())
                StartCoroutine(FireRoutine());
        }

        if ((Time.time - lastTimeOnTarget) >= turretSO.targetLoiterTime)
        {
            GetComponent<FindTarget>().SearchForTarget();
        }
    }
    
    protected virtual IEnumerator FireRoutine()
    {
        lastTimeOnTarget = Time.time;
        lastFireTime = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) {
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = platformTurret.rotation;
                projectile.SetActive(true);
            }
            
            if (turretSO.projectileSO.dischargePrefab != null)
                Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
                    Quaternion.identity);
            
            yield return new WaitForSeconds(turretSO.barrelFireDelay);
        }
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound(turretSO.fireSFX);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;    
        lastTimeOnTarget = Time.time;
        lastFireTime = Time.time;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretSO.projectileSO.range);
        
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, turretSO.minRange);
    }
}
