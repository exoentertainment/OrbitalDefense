using UnityEngine;

[CreateAssetMenu(fileName = "Enemy SO", menuName = "Enemy SO")]
public class EnemySO : ScriptableObject
{
    public LayerMask targetLayer;
    
    #region --Movement Variables--

    public float moveSpeed;
    public float turnSpeed;
    public int minMovementRadius;
    public int maxMovementRadius;

    #endregion

    #region --Health Variables--

    public int minHealth;
    public int maxHealth;
    
    public GameObject explosionPrefab;
    public float explosionFrequency;
    public float explosionDuration;
    public int numExplosions;

    #endregion

    public int pointValue;
    public AudioClipSO shipExplosion;
}
