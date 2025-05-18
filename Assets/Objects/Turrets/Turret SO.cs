using UnityEngine;

[CreateAssetMenu(fileName = "Turret SO", menuName = "Turret SO")]
public class TurretSO : ScriptableObject
{
    #region --Attack Variables--

    public BaseProjectileSO projectileSO;
    public AudioClipSO fireSFX;
    public float fireRate;
    public float baseTrackingSpeed;
    public int targetLoiterTime;
    public float barrelFireDelay;
    public LayerMask targetLayers;

    [Range(-1, 0)]
    public float trackingErrorMin;
    
    [Range(0, 1)]
    public float trackingErrorMax;
    
    public float GetTrackingError()
    {
        return Random.Range(trackingErrorMin, trackingErrorMax);
    }
    
    #endregion
}
