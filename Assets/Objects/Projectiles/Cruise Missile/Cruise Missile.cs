using System;
using System.Collections;
using UnityEngine;

public class CruiseMissile : MonoBehaviour, IDamageable
{
    [SerializeField] CruiseMissileSO missileSO;

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
        startRotation.eulerAngles = rb.rotation.eulerAngles;
    }

    private void OnEnable()
    {
        StartCoroutine(CoastPhaseRoutine());
        StartCoroutine(DisableRoutine());
    }

    private void OnDisable()
    {
        target = null;
        //rb.rotation = Quaternion.Euler(0, 0, 0);
        coastPhase = true;
    }

    private void FixedUpdate()
    {
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
            Coast();
            
            if ((Time.time - beginCoastPhase) > missileSO.coastDuration)
            {
                coastPhase = false;
            }
                
            yield return new WaitForEndOfFrame();
        }
    }
    
    void Move()
    {
        rb.linearVelocity = rb.rotation * Vector3.forward * (missileSO.speed * Time.fixedDeltaTime);
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
        rb.linearVelocity = transform.rotation * Vector3.forward * (missileSO.coastSpeed * Time.fixedDeltaTime);
    }
    
    public void SetTarget(GameObject target)
    {
        this.target = target;
        Debug.Log(target.gameObject.name);
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
    
    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(missileSO.duration);
        
        Instantiate(missileSO.impactPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
