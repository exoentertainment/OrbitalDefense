using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CruiseMissile : MonoBehaviour, IDamageable
{
    [SerializeField] CruiseMissileSO missileSO;
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

    GameObject target;
    private bool coastPhase = true;
    
    private Rigidbody rb;
    
    Quaternion startRotation = new Quaternion();
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(CoastPhaseRoutine());
        randomTimeOffset = Random.Range(0f, 1000f);
        startTime = Time.time;
    }

    private void OnEnable()
    {
        StartCoroutine(CoastPhaseRoutine());
        StartCoroutine(DisableRoutine());
    }

    private void OnDisable()
    {
        target = null;
        coastPhase = true;
    }

    private void FixedUpdate()
    {
        if(coastPhase)
            Coast();
        
        if (!coastPhase && target != null)
        {
            RotateTowardsTarget();
            Move();
        }
        else if (target == null)
        {
            FindClosestTarget();
            Move();
        }
    }

    IEnumerator CoastPhaseRoutine()
    {
        float beginCoastPhase = Time.time;
        
        while(coastPhase)
        {
            if ((Time.time - beginCoastPhase) >= missileSO.coastDuration)
            {
                coastPhase = false;
            }
                
            yield return new WaitForEndOfFrame();
        }
    }
    
    void Move()
    {
        if (target != null)
        {
            rb.linearVelocity = transform.rotation * Vector3.forward * (missileSO.speed * Time.fixedDeltaTime);
        }
        else
        {
            FindClosestTarget();
        }
    }

    void RotateTowardsTarget()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation,
            5 * Time.fixedDeltaTime));
    }
    
    void Coast()
    {
        rb.MovePosition(transform.position + transform.forward * (missileSO.coastSpeed * Time.fixedDeltaTime));
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
    
    void FindClosestTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity,
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
    
    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(missileSO.duration);
        
        Instantiate(missileSO.impactPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
