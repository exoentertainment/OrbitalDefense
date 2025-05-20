using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightMissile : MonoBehaviour, IDamageable
{
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
    
    [SerializeField] MissileSO missileSO;

    private Rigidbody rb;
    private GameObject target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        randomTimeOffset = Random.Range(0f, 1000f);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Move();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
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

            //rb.MoveRotation(Quaternion.Euler(wiggleX, wiggleY, wiggleZ) * rb.rotation);
            transform.rotation = Quaternion.Euler(wiggleX, wiggleY, wiggleZ) * transform.rotation;
            // rb.MovePosition(transform.position +(transform.rotation * Vector3.forward * (missileSO.speed * Time.deltaTime)));
            //rb.MovePosition(transform.position + (Vector3.forward * (missileSO.speed * Time.fixedDeltaTime)));
            rb.linearVelocity = transform.rotation * Vector3.forward * (missileSO.speed * Time.fixedDeltaTime);
            //transform.position += transform.rotation * Vector3.forward * (missileSO.speed * Time.fixedDeltaTime);
        }
        else
        {
            FindClosestTarget();
            rb.linearVelocity = transform.rotation * Vector3.forward * (missileSO.speed * Time.fixedDeltaTime);
        }
    }

    void FindClosestTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileSO.range,
            missileSO.targetLayers);

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
                    target = possibleTargets[x].gameObject;
                }
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(missileSO.impactPrefab, other.contacts[0].point, Quaternion.identity);
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(missileSO.damage);
        
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        Instantiate(missileSO.impactPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(DisableRoutine());
    }
    
    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(missileSO.duration);
        
        Instantiate(missileSO.impactPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
