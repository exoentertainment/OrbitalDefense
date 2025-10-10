using System;
using UnityEngine;
using UnityEngine.Events;

public class KamikazeAttack : MonoBehaviour
{
    #region -- Serialized Fields --
    
    [SerializeField] EnemySO enemySO;
    [SerializeField] private UnityEvent onDeath;
    
    #endregion

    private void Update()
    {
        //CheckArea();
    }

    void CheckArea()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, 5f, enemySO.targetLayer);

        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            foreach (Collider target in possibleTargets)
            {
                TryGetComponent<IDamageable>(out IDamageable targetHit);
                targetHit.TakeDamage(enemySO.maxHealth);
            }
            
            onDeath?.Invoke();
        }
    }
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Weapon Platform"))
    //     {
    //         Collider[] possibleTargets = Physics.OverlapSphere(transform.position, 5f, enemySO.targetLayer);
    //
    //         if (possibleTargets.Length > 0)
    //         {
    //             float closestEnemy = Mathf.Infinity;
    //
    //             foreach (Collider target in possibleTargets)
    //             {
    //                 TryGetComponent<IDamageable>(out IDamageable targetHit);
    //                 targetHit.TakeDamage(enemySO.maxHealth);
    //             }
    //         }
    //
    //         onDeath?.Invoke();
    //     }
    // }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.TryGetComponent<IDamageable>(out IDamageable targetHit);

        if (targetHit != null)
        {
            float currentHealth = transform.root.GetComponent<EnemyHealth>().GetHealth();
            targetHit.TakeDamage(enemySO.maxHealth - currentHealth);
        }

        onDeath?.Invoke();
    }
}
