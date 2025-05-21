using UnityEngine;
using System.Collections;

public class PlasmaTurret : BaseTurret
{
    [SerializeField] Light[] lights;
    
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

        foreach (Light light in lights)
        {

            light.enabled = true;
            Debug.Log(light.isActiveAndEnabled);
            yield return new WaitForSeconds(0.2f);
        }
        
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) 
            {
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = platformTurret.rotation;
                projectile.SetActive(true);
            }

            if (turretSO.projectileSO.dischargePrefab != null)
                Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position,
                    Quaternion.identity);
            
            yield return new WaitForSeconds(turretSO.barrelFireDelay);
        }
        
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound(turretSO.fireSFX);
    }

    void FlashLights()
    {
        
    }
}
