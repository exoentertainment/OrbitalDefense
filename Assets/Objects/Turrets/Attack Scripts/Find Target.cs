using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FindTarget : MonoBehaviour
{
    [FormerlySerializedAs("platformSO")] [SerializeField] TurretSO turretSO;
    [SerializeField] UnityEvent<GameObject> onTargetFound;
    [SerializeField] private bool isTargetRoot;
    
    GameObject target;

    private void Update()
    {
        if (target != null)
        {
            if (target.activeSelf)
            {
                CheckDistanceToTarget();
            }
            else
                target = null;
        }
        else
            SearchForTarget();
    }

    public virtual void SearchForTarget()
    {
        //Debug.Log("searching for target");
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, turretSO.projectileSO.range, turretSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);
                
                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    // target = possibleTargets[x].transform.root.gameObject;
                    target = possibleTargets[x].transform.gameObject;
                }
            }
        }

        if (target != null)
        {
            bool isRootTransform = false;

            while (!isRootTransform)
            {
                // if(target.transform.parent.gameObject != null)
                //     Debug.Log(target.transform.parent.gameObject.name);
                
                if (target.transform.parent != null)
                {
                    if (target.layer == target.transform.parent.gameObject.layer)
                        target = target.transform.parent.gameObject;
                    else
                        isRootTransform = true;
                }
                else
                {
                    isRootTransform = true;
                }
            }
            
            onTargetFound?.Invoke(target);
            // if(!isTargetRoot)
            //     onTargetFound?.Invoke(target);
            // else
            //     onTargetFound?.Invoke(target.transform.root.gameObject);
        }
    }
    
    protected virtual void CheckDistanceToTarget()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget > turretSO.projectileSO.range)
            {
                target = null;
                onTargetFound?.Invoke(null);
                SearchForTarget();
            }
        }
    }
}
