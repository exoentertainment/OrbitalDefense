using UnityEngine;
using System.Collections;

public class MissileLauncherTurret : BaseTurret
{
    void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    
    protected override IEnumerator FireRoutine()
    {
        lastTimeOnTarget = Time.time;
        lastFireTime = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) 
            {
                projectile.transform.position = spawnPoint.position;
                Vector3 targetDir = target.transform.position - spawnPoint.position;
                targetDir.Normalize();
                projectile.transform.rotation = Quaternion.LookRotation(targetDir);
                projectile.SetActive(true);
                projectile.GetComponent<LightMissile>().SetTarget(target);
            }

            if (turretSO.projectileSO.dischargePrefab != null)
                Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
                    platformTurret.transform.rotation);
            
            yield return new WaitForSeconds(turretSO.barrelFireDelay);
        }
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound(turretSO.fireSFX);
    }
}
