using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FlakProjectile : MonoBehaviour
{
    [SerializeField] BaseProjectileSO projectileSO;
    [FormerlySerializedAs("minExplodeRange")] [SerializeField] private float minExplodeTime;
    [FormerlySerializedAs("maxExplodeRange")] [SerializeField] float maxExplodeTime;
    [SerializeField] private float explodeRange;
    
    Rigidbody rigidbody;
    private float explodeTime;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rigidbody.MovePosition(transform.position + (transform.forward * (projectileSO.speed * Time.fixedDeltaTime)));
    }

    IEnumerator ExplodeRoutine()
    {
        yield return new WaitForSeconds(explodeTime);
        
        Instantiate(projectileSO.impactPrefab, transform.position, Quaternion.identity);
        Explode();
        gameObject.SetActive(false);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange, projectileSO.targetLayers);

        foreach (Collider hit in colliders)
        {
            hit.gameObject.GetComponent<IDamageable>()?.TakeDamage(projectileSO.damage);
        }
    }
    
    void ResetExplodeRange()
    {
        explodeTime = Random.Range(minExplodeTime, maxExplodeTime);
    }
    
    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.duration);
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ResetExplodeRange();
        StartCoroutine(ExplodeRoutine());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
}
