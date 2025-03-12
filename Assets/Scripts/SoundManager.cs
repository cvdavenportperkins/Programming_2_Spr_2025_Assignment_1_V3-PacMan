using UnityEngine;
using System.Collections;
using UnityEngine.Timeline;
using System.Runtime.CompilerServices;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource AS; //soundeffects
    public AudioSource powerupSource; //playerturbo SFX
    public AudioSource BGM; //Background music
    public AudioClip GameMusic;
    public AudioClip PlayerMoveSFX, EnemyMoveSFX, PlayerHitSFX, PlayerDieSFX, PlayerKillSFX, PlayerTurboSFX, ScoreUpSFX, ScoreDownSFX, TimerSFX, GameStartSFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic(GameMusic);
    }

    public void PlaySound(AudioClip clip)
    {
        AS.PlayOneShot(clip);
        Debug.Log("Playing SFX: " + clip.name);
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (clip != null)
        {
            BGM.clip = clip;
            BGM.loop = true;
            BGM.Play();
            Debug.Log("Playing BGM: " + clip.name);
        }
        else
        {
            Debug.LogError("BMG AudioClip is not assigned");
        }
    }
    public void PlayPowerupSound(AudioClip clip, float duration)
    {
        StartCoroutine(PlayPowerUpSoundRoutine(clip, duration));
    }    
    
    private IEnumerator PlayPowerUpSoundRoutine(AudioClip clip, float duration)
    { 
        // mute background music
        BGM.volume = 0;

        // Play power-up sound
        AS.volume = 1;
        powerupSource.volume = 1;
        powerupSource.clip = clip;
        powerupSource.loop = true;
        powerupSource.Play();
        Debug.Log("Playing PlayerTurbo sound: " + clip.name);

        // Stop power-up sound after duration and resume background music
        yield return new WaitForSeconds(duration);

        // Stop power-up sound
        powerupSource.Stop();

        // Resume background music
        BGM.volume = 1;
    }
}


