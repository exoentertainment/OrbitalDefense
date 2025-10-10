using System;
using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using Random = UnityEngine.Random;

public class AsteroidGrabberMovement : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Scriptable Object")] 
    [SerializeField] private EnemySO enemySO;
    

    [Header("Variables")]
    [SerializeField] private LayerMask planetLayer;
    [SerializeField] private float asteroidHaulSpeedModified = 1;
    [SerializeField] private int asteroidGrabRange;
    [SerializeField] private GameObject tractorBeam;
    
    #endregion

    private bool isDead;
    bool hasAsteroid;
    private GameObject target;
    private float currentAsteroidHaulSpeedModified = 1;
    
    Rigidbody rigidbody;
    private GameObject asteroid;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        FindClosestAsteroid();
    }

    private void Update()
    {
        if(!isDead)
            CheckDistanceToTarget();
        
        if(hasAsteroid)
            UpdateTractorBeam();
    }

    private void FixedUpdate()
    {
        if (!isDead && target != null)
        {
            RotateTowardsTarget();
            MoveTowardsTarget();
        }
    }

    void FindClosestAsteroid()
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
        else
            FindClosestPlanet();
    }

    public void FindClosestPlanet()
    {
        float closestEnemy = Mathf.Infinity;
        
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, planetLayer);
        
        if (potentialTargets.Length > 0)
        {
            for (int x = 0; x < potentialTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(potentialTargets[x].transform.position, transform.position);
                
                if (distanceToEnemy < closestEnemy)
                {
                    closestEnemy = distanceToEnemy;
                    target = potentialTargets[x].gameObject;
                }
            }

            currentAsteroidHaulSpeedModified = asteroidHaulSpeedModified;
        }
    }
    
    void RotateTowardsTarget()
    {
        rigidbody.MovePosition(transform.position + transform.forward * (enemySO.moveSpeed * Time.deltaTime));
    }

    void MoveTowardsTarget()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);

        rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation,
            enemySO.turnSpeed * Time.deltaTime));
    }

    void GrabAsteroid()
    {
        hasAsteroid = true;
        tractorBeam.SetActive(true);
        target.transform.SetParent(transform);
        asteroid = target;
        //target.GetComponent<MMAutoRotate>().enabled = false;
    }

    void CheckDistanceToTarget()
    {
        if(!hasAsteroid)
            if (Vector3.Distance(transform.position, target.transform.position) <= asteroidGrabRange)
            {
                GrabAsteroid();
                FindClosestPlanet();
            }
    }

    void UpdateTractorBeam()
    {
        tractorBeam.GetComponent<LineRenderer>().SetPosition(0, transform.position);    
        tractorBeam.GetComponent<LineRenderer>().SetPosition(1, asteroid.transform.position);    
    }
    
    public void SetDead()
    {
        isDead = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Planet"))
            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(enemySO.maxHealth);
        
        Instantiate(enemySO.explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, asteroidGrabRange);
    }
}
