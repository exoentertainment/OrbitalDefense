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
            // GameObject projectile = projectilePool.GetPooledObject(); 
            // if (projectile != null && target != null) 
            // {
            //     projectile.transform.position = spawnPoint.position;
            //     Vector3 targetDir = target.transform.position - spawnPoint.position;
            //     targetDir.Normalize();
            //     projectile.transform.rotation = Quaternion.LookRotation(targetDir);
            //     projectile.SetActive(true);
            //     projectile.GetComponent<LightMissile>().SetTarget(target);
            // }

            if (target != null)
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.PlaySound(turretSO.fireSFX);
                
                GameObject projectile = Instantiate(turretSO.projectileSO.projectilePrefab, spawnPoint.position,
                    platformTurret.rotation);
                
                Vector3 targetDir = target.transform.position - spawnPoint.position;
                targetDir.Normalize();
                projectile.transform.rotation = Quaternion.LookRotation(targetDir);
                
                projectile.TryGetComponent<LightMissile>(out LightMissile lightMissile);
                if(lightMissile != null)
                    lightMissile.SetTarget(target);
                
                projectile.TryGetComponent<PlasmaMissile>(out PlasmaMissile plasmaMissile);
                if(plasmaMissile != null)
                    plasmaMissile.SetTarget(target);
            }

            if (turretSO.projectileSO.dischargePrefab != null)
                Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
                    Quaternion.identity);
            
            yield return new WaitForSeconds(turretSO.barrelFireDelay);
        }
    }
}
