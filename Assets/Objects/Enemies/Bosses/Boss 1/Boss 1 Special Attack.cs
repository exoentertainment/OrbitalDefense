using System.Collections;
using UnityEngine;

public class Boss1SpecialAttack : MonoBehaviour
{
    [SerializeField] private TurretSO turretSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private int attackTimer;

    private bool isFiring;

    public void BeginAttackTimer()
    {
        if(!isFiring)
            StartCoroutine(BeginAttackTimerRoutine());
    }

    IEnumerator BeginAttackTimerRoutine()
    {
        isFiring = true;
        
        yield return new WaitForSeconds(attackTimer);
        
        if (turretSO.projectileSO.dischargePrefab != null)
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(turretSO.projectileSO.dischargePrefab, spawnPoint.position, Quaternion.identity);
                Instantiate(turretSO.projectileSO.projectilePrefab, spawnPoint.position, gameObject.transform.rotation);

                yield return new WaitForSeconds(turretSO.fireRate);
            }

        isFiring = false;
    }
}
