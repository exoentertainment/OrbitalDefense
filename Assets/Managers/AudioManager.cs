using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    #region --Clips--

    [Header("Audio Scriptable Objects")]
    [SerializeField] PlatformClipsSO platformClips;
    [SerializeField] private EnemyClipsSO enemyClips;
    [SerializeField] private WeaponSFXClips weaponSfxClips;
    [SerializeField] UIAudioClips uiAudioClips;
    [SerializeField] MusicClipsSO musicClips;
    [SerializeField] VFXClipsSO vfxClips;
    
    #endregion

    AudioSource musicAudioSource;

    public static AudioManager instance;

    private int levelOffset = 2;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicAudioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySound(AudioClipSO soundSO)
    {
        GameObject soundObject = new GameObject("Audio Clip Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = soundSO.clip;
        audioSource.loop = soundSO.loop;
        audioSource.volume = soundSO.volume;
        audioSource.pitch = soundSO.pitch;
        audioSource.outputAudioMixerGroup = soundSO.audioMixer;

        audioSource.Play();
        Destroy(soundObject, soundSO.clip.length);
    }

    public void PlayMusic(int track)
    {
        musicAudioSource.Stop();
        
        musicAudioSource.clip = musicClips.musicClip[track].clip;
        musicAudioSource.loop = musicClips.musicClip[track].loop;
        musicAudioSource.volume = musicClips.musicClip[track].volume;
        musicAudioSource.pitch = musicClips.musicClip[track].pitch;
        musicAudioSource.outputAudioMixerGroup = musicClips.musicClip[track].audioMixer;

        musicAudioSource.Play();
    }

    #region --Platform Turret Sounds --

    public void PlayTurretSound()
    {
        PlaySound(platformClips.basicTurret);
    }

    public void PlayLightMissileTurretSound()
    {
        PlaySound(platformClips.lightMissileTurret);
    }
    
    public void PlayCruiseMissileTurretSound()
    {
        PlaySound(platformClips.cruiseMissileTurret);
    }
    
    public void PlayPlasmaTurretSound()
    {
        PlaySound(platformClips.plasmaTurret);
    }

    public void PlayPlasmaMissileTurretSound()
    {
        PlaySound(platformClips.plasmaMissileTurret);
    }

    #endregion

    #region -- Enemy Sounds --

    public void PlayEnemyTurretSound()
    {
        PlaySound(enemyClips.basicTurret);
    }
    
    public void PlayEnemyRotarySound()
    {
        PlaySound(enemyClips.rotaryTurret);
    }

    public void PlayEnemySmallExplosion()
    {
        PlaySound(enemyClips.smallExplosion);
    }
    
    public void PlayPlasmaBossWeaponCharging()
    {
        PlaySound(enemyClips.plasmaBossSpecialWeaponCharging);
    }
    
    public void PlayPlasmaBossWeaponFire()
    {
        PlaySound(enemyClips.plasmaBossSpecialWeapon);
    }

    public void PlayMassDriverBossWeaponFire()
    {
        PlaySound(enemyClips.massDriverSpecialWeaponFire);
    }
    
    public void PlayPulseLaserBossWeaponFire()
    {
        PlaySound(enemyClips.pulseLaserWeaponFire);
    }
    
    #endregion

    #region -- Explosion SFX --

    public void PlaySmallExplosion()
    {
        PlaySound(weaponSfxClips.smallExplosion);
    }

    public void PlayLargeExplosion()
    {
        PlaySound(weaponSfxClips.largeExplosion);
    }
    
    #endregion

    #region -- General SFX --

    public void PlayPlatformExplosion()
    {
        PlaySound(platformClips.platformExplosion);
    }

    public void PlayLevelCleared()
    {
        PlaySound(vfxClips.levelCleared);
    }
    
    public void PlayGameOver()
    {
        PlaySound(vfxClips.gameOver);
    }

    #endregion
    
    #region -- UI SFX --

    public void PlayUISelect()
    {
        PlaySound(uiAudioClips.selectSound);
    }
    
    public void PlayUIClose()
    {
        PlaySound(uiAudioClips.closeSound);
    }
    
    public void PlayUIError()
    {
        PlaySound(uiAudioClips.errorSound);
    }

    #endregion
}
