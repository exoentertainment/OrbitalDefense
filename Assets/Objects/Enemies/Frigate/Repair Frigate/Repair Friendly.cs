using System;
using UnityEngine;

public class RepairFriendly : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private int repairAmount;
    [SerializeField] float repairTime;
    [SerializeField] private float repairDistance;
    [SerializeField] LayerMask targetLayer;

    [Header("Components")]
    [SerializeField] ParticleSystem repairParticles;
    
    #endregion
    
    float lastRepairTime;
    GameObject target;

    private void Start()
    {
        lastRepairTime = Time.time;
    }

    private void Update()
    {
        if (target != null)
        {
            CheckDistanceToTarget();
            RepairTarget();
        }
        else
            FindTarget();
    }

    void FindTarget()
    {
        float closestEnemy = Mathf.Infinity;

        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, repairDistance, targetLayer);

        if (potentialTargets.Length > 0)
        {
            for (int x = 0; x < potentialTargets.Length; x++)
            {
                if (potentialTargets[x].transform.root.gameObject == transform.root.gameObject)
                    continue;

                float distanceToEnemy =
                    Vector3.Distance(potentialTargets[x].transform.position, transform.position);

                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    target = potentialTargets[x].transform.root.gameObject;
                }
            }
        }
    }

    void RepairTarget()
    {
        if ((Time.time - lastRepairTime) > repairTime)
        { 
            lastRepairTime = Time.time;

            if(target != null)
                if (target.TryGetComponent<IRepairable>(out IRepairable repairTarget))
                {
                    if (repairTarget.GetHealth() < 1)
                    {
                        repairParticles.gameObject.transform.LookAt(target.transform);
                        repairParticles.Play();

                        repairTarget.RepairHealth(repairAmount);
                    }

                    if (repairTarget.GetHealth() >= 1)
                        target = null;
                }
        }
    }

    void CheckDistanceToTarget()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > repairDistance)
            target = null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, repairDistance);
    }
}
