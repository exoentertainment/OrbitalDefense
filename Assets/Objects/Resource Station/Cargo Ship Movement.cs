using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CargoShipMovement : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] 
    [SerializeField] private CargoShipScriptableObject cargoShipSO;

    #endregion

    BoxCollider collider;
    private GameObject originStation;
    private GameObject target;
    
    Rigidbody rigidbody;

    private bool isFloating = true;
    
    [Tooltip("The maximum amount of swarm.")]
    [SerializeField] protected float maxSwarmAmount = 20;

    [Tooltip("The distance from the target over which the swarm fades to zero (so that the missile can aim at the target as it gets close).")]
    [SerializeField] protected float swarmFadeDistance = 100;

    [Tooltip("The swarm frequency (how rapidly it weaves from one side to the other).")]
    [SerializeField] protected float swarmFrequency = 2;

    [Tooltip("The maximum amount of steering power (applied as a lerp) to guide the missile to the target.")]
    [SerializeField] protected float guidanceSteeringPower = 5;

    [Tooltip("The time from when the missile is launched to when the swarm level is zero (necessary to prevent the missile from sometimes getting stuck in a swarm behaviour that carries it away from the target).")]
    [SerializeField] protected float swarmFadeTime = 5;

    protected float startTime;
    protected float randomTimeOffset;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        randomTimeOffset = Random.Range(0f, 1000f);
        startTime = Time.time;
        
        StartCoroutine(FloatShipRoutine());
    }

    private void Update()
    {
        if(target == null && !isFloating)
            FindNearestStation();
    }

    private void FixedUpdate()
    {
        if (target != null && !isFloating)
        {
            //Rotate();
            MoveTowardsTarget();
        }
    }

    public void SetOriginStation(GameObject station)
    {
        originStation = station;
        transform.rotation = originStation.transform.rotation;
    }

    void FindNearestStation()
    {
        float closestEnemy = Mathf.Infinity;

        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, cargoShipSO.resourceStationLayerMask);
        // List<Collider> list = new List<Collider>();
        // foreach (Collider potentialTarget in potentialTargets)
        // {
        //     bool isRootTransform = false;
        //
        //     while (!isRootTransform)
        //     {
        //         if (potentialTarget.transform.parent != null)
        //         {
        //             if (potentialTarget.gameObject.layer == potentialTarget.transform.parent.gameObject.layer)
        //                 if (potentialTarget.transform.parent.gameObject != originStation)
        //                     list.Add(potentialTarget);
        //                 else
        //                     isRootTransform = true;
        //         }
        //         else
        //         {
        //             isRootTransform = true;
        //         }
        //     }
        // }
        //
        // if (list.Count > 0)
        // {
        //     target = list[Random.Range(0, potentialTargets.Length)].transform.gameObject;
        // }
            
        if (potentialTargets.Length > 0)
        {
            bool isTargetFound = false;
            bool isRootTransform = false;
            int randomTarget;
            
                target = potentialTargets[Random.Range(0, potentialTargets.Length)].gameObject;
                
                while (!isRootTransform)
                {
                    if (target.transform.parent != null)
                    {
                        if (target.gameObject.layer == target.transform.parent.gameObject.layer)
                            target = target.transform.parent.gameObject;
                        else
                            isRootTransform = true;
                    }
                    else
                    {
                        isRootTransform = true;
                    }
                }
            
                if(target == originStation)
                    FindNearestStation();
            // for (int x = 0; x < potentialTargets.Length; x++)
            // {
            //     float distanceToEnemy =
            //         Vector3.Distance(potentialTargets[x].transform.position, transform.position);
            //
            //     if (distanceToEnemy < closestEnemy)
            //     {
            //         //target = potentialTargets[Random.Range(1, potentialTargets.Length)].transform.gameObject;
            //         
            //         if (potentialTargets[x].transform.gameObject.layer == originStation.layer)
            //             continue;
            //         
            //         closestEnemy = distanceToEnemy;
            //         target = potentialTargets[x].transform.gameObject;
            //     }
            // }
        }
        
        // bool isRootTransform = false;
        //
        // while (!isRootTransform)
        // {
        //     if (target.transform.parent != null)
        //     {
        //         if (target.layer == target.transform.parent.gameObject.layer)
        //             target = target.transform.parent.gameObject;
        //         else
        //             isRootTransform = true;
        //     }
        //     else
        //     {
        //         isRootTransform = true;
        //     }
        // }
    }
    
    void Rotate()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        rigidbody.MoveRotation(Quaternion.SlerpUnclamped(transform.rotation, targetRotation,
            cargoShipSO.turnSpeed * Time.deltaTime));
    }
    
    void MoveTowardsTarget()
    {
        //rigidbody.linearVelocity = transform.rotation * Vector3.forward * (cargoShipSO.moveSpeed * Time.fixedDeltaTime);
        //rigidbody.MovePosition(transform.position + transform.forward * (cargoShipSO.moveSpeed * Time.fixedDeltaTime));
        
        if (target != null)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            float swarmAmount = dist > swarmFadeDistance ? 1 : (dist / swarmFadeDistance);
            swarmAmount *= Mathf.Clamp(1 - ((Time.time - startTime) / swarmFadeTime), 0, 1);

            Vector3 fwd = transform.forward;
            Vector3 toTarget = (target.transform.position - transform.position).normalized;
            fwd = Vector3.Lerp(fwd, toTarget, (1 - swarmAmount) * guidanceSteeringPower * Time.fixedDeltaTime);
            transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);

            float wiggleX = swarmAmount * (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.2f) - 0.5f) * maxSwarmAmount;
            float wiggleY = swarmAmount * (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.5f) - 0.5f) * maxSwarmAmount;
            float wiggleZ = swarmAmount * (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.8f) - 0.5f) * maxSwarmAmount;
            
            transform.rotation = Quaternion.Euler(wiggleX, wiggleY, wiggleZ) * transform.rotation;

            rigidbody.MovePosition(transform.position + transform.forward * (cargoShipSO.moveSpeed * Time.fixedDeltaTime));
        }
    }
    
    IEnumerator FloatShipRoutine()
    {
        float floatTime = 0;

        while (isFloating)
        {
            rigidbody.MovePosition(transform.position + transform.forward * (cargoShipSO.moveSpeed * Time.fixedDeltaTime));
            
            floatTime += Time.deltaTime;
            if (floatTime >= cargoShipSO.moveDelay)
            {
                isFloating = false;
                FindNearestStation();
            }

            yield return new WaitForFixedUpdate();
        }
        
        collider.enabled = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (collider.enabled)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Resource Station"))
            {
                ResourceManager.instance.IncreaseResources(cargoShipSO.resourceAmount);
                Destroy(gameObject);
            }
        }
    }
}
