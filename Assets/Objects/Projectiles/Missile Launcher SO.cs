using UnityEngine;

[CreateAssetMenu(fileName = "Missile Launcher SO", menuName = "Missile Launcher SO")]
public class MissileLauncherSO : ScriptableObject
{
    public float fireRate;
    public float baseTrackingSpeed;
    public float barrelTrackingSpeed;
    public int targetLoiterTime;
    public float barrelFireDelay;
    public LayerMask targetLayers;
    
    public BaseProjectileSO projectileSO;
    public AudioClipSO fireSFX;
    public GameObject weaponPrefab;

    public MissileType targettingType;
    //public MissileSO missileSO;

    public enum MissileType
    {
        SingleTarget,
        MultiTarget
    }
}
