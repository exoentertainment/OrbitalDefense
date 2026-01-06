using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Boss5Movement : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private int raycastRange;
    
    [SerializeField] private int specialAttackRange;
    [SerializeField] private int specialAttackTimer;
    [SerializeField] private UnityEvent specialAttack;

    private GameObject target;
    Vector3 targetPos;
    private Vector3 randomSpot;
    Rigidbody rigidbody;
    
    private bool isDead;
    private bool isUsingSpecialWeapon;

    private float lastSpecialAttackTime;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        lastSpecialAttackTime = Time.time;
    }

    private void Update()
    {
        if(!isDead && target == null)
        {
            SetNewTarget();
        }

        if (!isDead & target != null)
        {
            CheckDistanceToTarget();
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            if (!isDead && !isUsingSpecialWeapon)
            {
                UpdateTargetPosition();
                MoveTowardsTarget();
                RotateTowardsTarget();
            }
            else if (!isDead && isUsingSpecialWeapon)
            {
                RotateTowardsPlatform();
                specialAttack?.Invoke();
            }
        }
        
        if(isDead)
        {
            MoveForward();
        }     
    }

    public void SetDead()
    {
        isDead = true;
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
        
        if(target != null)
            SetNewPosition();
    }

    void SetSpecialWeaponTarget()
    {
        
    }

    void SetNewPosition()
    {
        Vector3 potentialPos;
        bool foundTarget = false;

        while (!foundTarget)
        {
            randomSpot = (Random.onUnitSphere * enemySO.minMovementRadius);
            potentialPos = randomSpot + target.transform.position;
            
            targetPos = potentialPos;
            foundTarget = true;
        }
    }

    void UpdateTargetPosition()
    {
        targetPos = target.transform.position + randomSpot;    
    }
    
    void MoveTowardsTarget()
    {
        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }

    void RotateTowardsTarget()
    {
        Vector3 targetVector = targetPos - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        rigidbody.MoveRotation(Quaternion.SlerpUnclamped(transform.rotation, targetRotation,
            enemySO.turnSpeed * Time.deltaTime));
    }

    void RotateTowardsPlatform()
    {
        Vector3 targetVector = target.transform.parent.parent.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        rigidbody.MoveRotation(Quaternion.SlerpUnclamped(transform.rotation, targetRotation,
            enemySO.turnSpeed * Time.deltaTime));
    }
    
    void CheckDistanceToTarget()
    {
        if (Vector3.Distance(transform.position, targetPos) <= 5 || target == null)
            SetNewPosition();

        if (Vector3.Distance(transform.position, target.transform.position) <= specialAttackRange &&
            (Time.time - lastSpecialAttackTime) > specialAttackTimer)
        {
            lastSpecialAttackTime = Time.time;
            isUsingSpecialWeapon = true;
            StartCoroutine(ResetMovementRoutine());
        }
    }

    void MoveForward()
    {
        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }

    IEnumerator ResetMovementRoutine()
    {
        yield return new WaitForSeconds(3);
        
        isUsingSpecialWeapon = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemySO.minMovementRadius);
    }
}
