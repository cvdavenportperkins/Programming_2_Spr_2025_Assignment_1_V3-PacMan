using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource AS;
    public AudioSource powerupSource;
    public AudioClip GameMusic, PlayerMoveSFX, EnemyMoveSFX, PlayerHitSFX, PlayerDieSFX, PlayerKillSFX, PlayerTurboSFX, ScoreUpSFX, ScoreDownSFX, TimerSFX, GameStartSFX;

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
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        AS.clip = clip;
        AS.loop = true;
        AS.Play();
    }
    public void PlayPowerupSound(AudioClip clip, float duration)
    {
        // Stop background music
        AS.Stop();

        // Play power-up sound
        powerupSource.clip = clip;
        powerupSource.loop = true;
        powerupSource.Play();

        // Start coroutine to stop power-up sound after duration and resume background music
        StartCoroutine(StopPowerupSoundAfterDuration(duration));
    }

    private System.Collections.IEnumerator StopPowerupSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Stop power-up sound
        powerupSource.Stop();

        // Resume background music
        PlayBackgroundMusic(GameMusic);
    }
}


