using UnityEngine;

[CreateAssetMenu(fileName = "New Planet Health SO", menuName = "Planet Health SO")]
public class PlanetHealthSO : ScriptableObject
{
    public int maxHealth;
    public int explosionDuration;
    public float delayPerExplosion;
    public GameObject explosionPrefab;
}
