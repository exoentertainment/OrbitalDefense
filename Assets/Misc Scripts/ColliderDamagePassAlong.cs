using UnityEngine;

public class ColliderDamagePassAlong : MonoBehaviour,IDamageable
{
    public void TakeDamage(float damage)
    {
        transform.root.TryGetComponent<IDamageable>(out IDamageable targetHit);
        if (targetHit != null)
            targetHit.TakeDamage(damage);
    }
}
