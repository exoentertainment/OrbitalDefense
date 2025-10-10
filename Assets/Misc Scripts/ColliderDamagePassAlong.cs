using UnityEngine;

public class ColliderDamagePassAlong : MonoBehaviour,IDamageable
{
    [SerializeField] private GameObject rootObject;
    
    public void TakeDamage(float damage)
    {
        rootObject.TryGetComponent<IDamageable>(out IDamageable targetHit);
        if (targetHit != null)
        {
            targetHit.TakeDamage(damage);
        }
    }
}
