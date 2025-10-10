using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class InterstellarMissile : MonoBehaviour, IDamageable
{
    [SerializeField] CruiseMissileSO missileSO;
    [SerializeField] private int maxHealth;
    
    GameObject target;
    private bool coastPhase = true;
    private float curentHealth;
    
    private Rigidbody rb;
    
    private bool isBeingHit;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(CoastPhaseRoutine());
        curentHealth = maxHealth;
    }

    private void Update()
    {
        if(isBeingHit)
            isBeingHit = false;
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
            rb.MovePosition(transform.position + transform.forward * (missileSO.speed * Time.fixedDeltaTime));
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
        
        if(other.gameObject == target)
            Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        curentHealth -= damage;
        isBeingHit = true;

        if (curentHealth <= 0)
        {
            Instantiate(missileSO.impactPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
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
}
