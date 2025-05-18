using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Enemy Clips", menuName = "Audio SO/Enemy Clips")]
public class EnemyClipsSO : ScriptableObject
{
    public AudioClipSO basicTurret;
    public AudioClipSO lightMissileTurret;
    public AudioClipSO rotaryTurret;
    public AudioClipSO smallExplosion;

    #region -- Boss SFX --

    [FormerlySerializedAs("plasmaBoss1SpecialWeapon")] [FormerlySerializedAs("boss1SpecialWeapon")] public AudioClipSO plasmaBossSpecialWeapon;
    [FormerlySerializedAs("boss1SpecialWeaponCharging")] public AudioClipSO plasmaBossSpecialWeaponCharging;

    public AudioClipSO massDriverSpecialWeaponFire;

    public AudioClipSO pulseLaserWeaponFire;

    #endregion
}
