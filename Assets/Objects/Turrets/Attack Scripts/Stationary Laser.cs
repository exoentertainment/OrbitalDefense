using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

public class StationaryLaser : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] protected TurretSO turretSO;
    [SerializeField] private Transform spawnPoint;
    [FormerlySerializedAs("lineRenderers")] [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject laserImpactPrefab;
    [SerializeField] private float laserDuration;
    [SerializeField] private int laserRange;

    #endregion
    
    GameObject target;
    float lastFireTime;
    bool isFiring;
    
    void Update()
    {
        if (target != null)
        {
            Fire();
        
            if (target.activeSelf)
            {
                UpdateLineRenderer();
            }
        }
    }
    
    public void SetTarget(GameObject target)
    {
        this.target = target;    
        lastFireTime = Time.time;
    }
    
    void UpdateLineRenderer()
    {
        if (isFiring)
        {
            float distanceToTarget = Vector3.Distance(spawnPoint.transform.position, target.transform.position);
            
            lineRenderer.SetPosition(1,spawnPoint.transform.position + (spawnPoint.transform.forward * distanceToTarget));
                lineRenderer.SetPosition(0, spawnPoint.transform.position);
        }
    }
    
    void Fire()
    {
        if ((Time.time - lastFireTime) > turretSO.fireRate)
        {
			Debug.unityLogger.Log("Firing laser");
            StartCoroutine(FireRoutine());
        }
    }
    
    IEnumerator FireRoutine()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(SpawnLaserHitsRoutine());
            
            lineRenderer.enabled = true;
            
            yield return new WaitForSeconds(laserDuration);
        
            isFiring = false;
            lastFireTime = Time.time;

            lineRenderer.enabled = false;
        }
    }

    IEnumerator SpawnLaserHitsRoutine()
    {
        while (isFiring)
        {
            if(Physics.Linecast(spawnPoint.position, spawnPoint.position + (spawnPoint.transform.forward * turretSO.projectileSO.range), out RaycastHit hit))
            {
                if(laserImpactPrefab != null)
                    Instantiate(laserImpactPrefab, hit.point, Quaternion.identity);
            }
            
            target.TryGetComponent<IDamageable>(out IDamageable targetHit);
            if (targetHit != null)
                targetHit.TakeDamage(turretSO.projectileSO.damage * Time.deltaTime);
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
