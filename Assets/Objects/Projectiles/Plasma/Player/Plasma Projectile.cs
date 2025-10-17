using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlasmaProjectile : MonoBehaviour
{
    [SerializeField] BaseProjectileSO projectileSO;
    [SerializeField] private int damageRadius;

    [SerializeField] float collateralDamagePercent;
    
    Rigidbody rigidbody;

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
        rigidbody.MovePosition(rigidbody.position + (transform.forward * (projectileSO.speed * Time.fixedDeltaTime)));
    }

    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.duration);
        
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    { 
        if(projectileSO.impactPrefab != null)
            Instantiate(projectileSO.impactPrefab, other.contacts[0].point, Quaternion.identity);
        
        //other.gameObject.GetComponent<IDamageable>()?.TakeDamage(missileSO.damage);
        Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, projectileSO.targetLayers);
        List<GameObject> possibleCollateralTargets =  new List<GameObject>();
        
        foreach (Collider hit in hits)
        {
            possibleCollateralTargets.Add(hit.transform.root.gameObject);
        }
        
        List<GameObject> collateralTargets = possibleCollateralTargets.Distinct().ToList();
        
        foreach (GameObject collateralTarget in collateralTargets)
        {
            collateralTarget.GetComponent<IDamageable>()?.TakeDamage((projectileSO.damage * collateralDamagePercent));
        }

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
    }
}
