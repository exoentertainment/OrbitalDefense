using UnityEngine;

[CreateAssetMenu(fileName = "Turret SO", menuName = "Turret SO")]
public class TurretSO : ScriptableObject
{
    #region --Attack Variables--

    public BaseProjectileSO projectileSO;
    public AudioClipSO fireSFX;
    public float fireRate;
    public float baseTrackingSpeed;
    public float barrelFireDelay;
    public LayerMask targetLayers;
    
    #endregion
}
