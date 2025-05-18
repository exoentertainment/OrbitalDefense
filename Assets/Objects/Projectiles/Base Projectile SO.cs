using UnityEngine;

[CreateAssetMenu(fileName = "Base Projectile SO", menuName = "Base Projectile SO")]
public class BaseProjectileSO : ScriptableObject
{
    public float speed;
    public float damage;
    public int duration;
    public int range;
    
    public LayerMask targetLayers;
    public GameObject projectilePrefab;
    public GameObject dischargePrefab;
    public GameObject impactPrefab;
}
