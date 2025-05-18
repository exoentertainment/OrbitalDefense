using UnityEngine;

[CreateAssetMenu(fileName = "Enemy SO", menuName = "Enemy SO")]
public class EnemySO : ScriptableObject
{
    public LayerMask targetLayer;
    
    #region --Movement Variables--

    public float moveSpeed;
    public float deathSpeed;

    public float turnSpeed;
    public int movementRadius;

    #endregion

    #region --Health Variables--

    public int maxHealth;
    
    public GameObject explosionPrefab;
    public float explosionDuration;
    public float explosionFrequency;
    
    public GameObject reactorExplosionPrefab;

    #endregion
}
