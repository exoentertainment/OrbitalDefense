using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private int raycastRange;
    //[SerializeField] private GameObject markerPrefab;

    private GameObject target;
    Vector3 targetPos;
    private Vector3 randomSpot;
    Rigidbody rigidbody;
    
    private bool isDead;
    protected float startTime;
    protected float randomTimeOffset;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        randomTimeOffset = Random.Range(0f, 1000f);
        startTime = Time.time;
        //marker = Instantiate(markerPrefab, transform.position, Quaternion.identity);
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
            if (!isDead)
            {
                UpdateTargetPosition();
                MoveTowardsTarget();
                RotateTowardsTarget();
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
            target = possibleTargets[Random.Range(0, possibleTargets.Length)].gameObject;
            // float closestEnemy = Mathf.Infinity;
            //
            // for (int x = 0; x < possibleTargets.Length; x++)
            // {
            //     target = possibleTargets[Random.Range(0, possibleTargets.Length)].gameObject;
            //     
            //     // float distanceToEnemy =
            //     //     Vector3.Distance(possibleTargets[x].transform.position, transform.position);
            //     //
            //     // if (distanceToEnemy < closestEnemy)
            //     // {
            //     //     closestEnemy = distanceToEnemy;
            //     //     // target = possibleTargets[x].transform.root.gameObject;
            //     //     target = possibleTargets[x].transform.gameObject;
            //     //     startTime =  Time.time;
            //     // }
            // }
        }
        
        if(target != null)
            SetNewPosition();
    }

    void SetNewPosition()
    {
        Vector3 potentialPos;

        randomSpot = (Random.insideUnitSphere * Random.Range(enemySO.minMovementRadius, enemySO.maxMovementRadius));
        potentialPos = randomSpot + target.transform.position;
        
        // if (IsLoSClear(potentialPos))
        // {
            targetPos = potentialPos;
        //}
    }
    
    bool IsLoSClear(Vector3 pos)
    {
        // if (!Physics.Raycast(raycastOrigin.position, pos - raycastOrigin.position, out RaycastHit hit, raycastRange))
        // {
        //     return true;
        // }
        //
        // return false;
        
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.transform.forward * raycastRange, out RaycastHit hit, raycastRange))
        {
            return false;
        }
        
        return true;
    }

    void UpdateTargetPosition()
    {
        targetPos = target.transform.position + randomSpot;    
        //marker.transform.position = targetPos;
    }
    
    void MoveTowardsTarget()
    {
        //rigidbody.MovePosition(rigidbody.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));

        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.fixedDeltaTime));
    }

    void RotateTowardsTarget()
    {
        float dist = Vector3.Distance(transform.position, targetPos);
        
        // if (dist < enemySO.evadeDistance)
        // {
        //     Debug.Log("evade");
        //     float swarmAmount = dist > swarmFadeDistance ? 1 : (dist / swarmFadeDistance);
        //     swarmAmount *= Mathf.Clamp(1 - ((Time.time - startTime) / swarmFadeTime), 0, 1);
        //
        //     Vector3 fwd = transform.forward;
        //     Vector3 toTarget = (targetPos - transform.position).normalized;
        //     fwd = Vector3.Lerp(fwd, toTarget, (1 - swarmAmount) * guidanceSteeringPower * Time.fixedDeltaTime);
        //     transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);
        //
        //     float wiggleX = swarmAmount *
        //                     (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.2f) - 0.5f) *
        //                     maxSwarmAmount;
        //     float wiggleY = swarmAmount *
        //                     (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.5f) - 0.5f) *
        //                     maxSwarmAmount;
        //     float wiggleZ = swarmAmount *
        //                     (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.8f) - 0.5f) *
        //                     maxSwarmAmount;
        //
        //     transform.rotation = Quaternion.Euler(wiggleX, wiggleY, wiggleZ) * transform.rotation;
        // }
        // else
        // {
        Vector3 targetVector = targetPos - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);
        
        rigidbody.MoveRotation(Quaternion.SlerpUnclamped(transform.rotation, targetRotation,
            enemySO.turnSpeed * Time.deltaTime));
        // }
    }

    void CheckDistanceToTarget()
    {
        if (Vector3.Distance(transform.position, targetPos) <= 5 || target == null)
            SetNewPosition();
    }

    void MoveForward()
    {
        rigidbody.MovePosition(rigidbody.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemySO.minMovementRadius);
    }
}
