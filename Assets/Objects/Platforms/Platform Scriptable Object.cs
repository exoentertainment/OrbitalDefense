using UnityEngine;

[CreateAssetMenu(fileName = "Platform SO", menuName = "Platform SO")]
public class PlatformScriptableObject : ScriptableObject
{
    public int maxHealth;
    public int resourceCost;
    public GameObject platformPrefab;
    public GameObject explosionPrefab;
    public float delayBetweenExplosions;
    public string platformDescription;
}