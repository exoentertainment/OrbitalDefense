using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class KamikazeMovement : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private int raycastRange;

    private GameObject target;
    Vector3 targetPos;
    private Vector3 randomSpot;
    Rigidbody rigidbody;
    
    private bool isDead;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if(!isDead && target == null)
        {
            SetNewTarget();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead && target != null)
        {
            RotateTowardsTarget();
            MoveTowardsTarget();
        }
        else
        {
            MoveForward();
        }
    }

    void SetNewTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, enemySO.targetLayer);
        
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
                    target = possibleTargets[x].transform.gameObject;
                }
            }
        }

        if (target != null)
        {
            SphereCollider collider =  target.GetComponent<SphereCollider>();
            targetPos = Random.insideUnitSphere * collider.radius;
        }
    }
    
    void MoveTowardsTarget()
    {
        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }

    void RotateTowardsTarget()
    {
        Vector3 targetVector = (target.transform.position + targetPos) - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation,
            enemySO.turnSpeed * Time.deltaTime));
    }
    
    void MoveForward()
    {
        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }
    
    public void SetDead()
    {
        isDead = true;
    }
}
