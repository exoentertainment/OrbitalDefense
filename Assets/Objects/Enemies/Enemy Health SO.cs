using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Health SO", menuName = "Enemy Health SO")]
public class EnemyHealthSO : ScriptableObject
{
    public int maxHealth;
    
    public GameObject explosionPrefab;
    public float explosionDuration;
    public float explosionFrequency;
    
    public GameObject reactorExplosionPrefab;
}
