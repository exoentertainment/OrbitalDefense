using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectPool))]
public class CruiseMissileLauncher : MonoBehaviour
{
    [SerializeField] protected TurretSO turretSO;
    [SerializeField] protected Transform[] spawnPoints;
    
    protected GameObject target;
    private float lastFireTime;
    
    protected ObjectPool projectilePool;

    protected void Awake()
    {
        projectilePool = GetComponent<ObjectPool>(); ;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    protected void Update()
    {
        if (target != null)
        {
            Fire();
        }
    }

    void Fire()
    {
        if ((Time.time - lastFireTime) > turretSO.fireRate)
        {
            StartCoroutine(FireRoutine());
        }
    }
    
    protected virtual IEnumerator FireRoutine()
    {
        lastFireTime = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) {
                projectile.SetActive(true);
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = spawnPoint.rotation;

                projectile.GetComponent<CruiseMissile>().SetTarget(target);
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
        lastFireTime = Time.time;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretSO.projectileSO.range);
    }
}
