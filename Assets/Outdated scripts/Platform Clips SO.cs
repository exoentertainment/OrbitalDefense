using UnityEngine;

[CreateAssetMenu(fileName = "Platform Clips", menuName = "Audio SO/Platform Clips")]
public class PlatformClipsSO : ScriptableObject
{
    public AudioClipSO basicTurret;
    public AudioClipSO lightMissileTurret;
    public AudioClipSO rotaryTurret;
    public AudioClipSO cruiseMissileTurret;
    public AudioClipSO plasmaTurret;
    public AudioClipSO plasmaMissileTurret;
    public AudioClipSO platformExplosion;
}
