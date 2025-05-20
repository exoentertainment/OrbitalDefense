using UnityEngine;
using System.Collections;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] BaseProjectileSO projectileSO;
    
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
        rigidbody.MovePosition(transform.position + (transform.forward * (projectileSO.speed * Time.fixedDeltaTime)));
    }

    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.duration);
        
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    { 
        if(projectileSO.impactPrefab != null)
            Instantiate(projectileSO.impactPrefab, other.GetContact(0).point, Quaternion.identity);
        
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(projectileSO.damage);
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
    }
}
