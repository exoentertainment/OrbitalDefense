using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class LaserTurret : BaseTurret
{
    #region --Serialized Fields--

    [FormerlySerializedAs("lineRenderer")] [SerializeField] LineRenderer[] lineRenderers;
    [SerializeField] GameObject laserImpactPrefab;
    [SerializeField] private float laserDuration;
    [SerializeField] private int laserRange;

    #endregion
    

    bool isFiring;
    
    void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (target != null)
        {
            if (target.activeSelf)
            {
                UpdateLineRenderer();
            }
        }
    }
    
    void UpdateLineRenderer()
    {
        if (isFiring)
        {
            float distanceToTarget = Vector3.Distance(spawnPoints[0].transform.position, target.transform.position);

            for(int x = 0; x < spawnPoints.Length; x++)
            {
                lineRenderers[x].SetPosition(1,
                    spawnPoints[x].transform.position + (spawnPoints[x].transform.forward * distanceToTarget));
                lineRenderers[x].SetPosition(0, spawnPoints[x].transform.position);
            }
        }
    }
    
    protected override IEnumerator FireRoutine()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(SpawnLaserHitsRoutine());

            for (int x = 0; x < lineRenderers.Length; x++)
            {
                lineRenderers[x].enabled = true;
            }

            lastTimeOnTarget = Time.time;
            
            yield return new WaitForSeconds(laserDuration);
        
            isFiring = false;
            lastFireTime = Time.time;

            for (int x = 0; x < lineRenderers.Length; x++)
            {
                lineRenderers[x].enabled = false;
            }
        }
    }

    IEnumerator SpawnLaserHitsRoutine()
    {
        while (isFiring)
        {
            if(Physics.Linecast(raycastOrigin.position, raycastOrigin.position + (raycastOrigin.transform.forward * turretSO.projectileSO.range), out RaycastHit hit))
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

    public void SetTarget(GameObject target)
    {
        this.target = target;    
        lastTimeOnTarget = Time.time;
        lastFireTime = Time.time;
    }
}
