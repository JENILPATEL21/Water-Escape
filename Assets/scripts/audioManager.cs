using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;
    public AudioClip bgmusic;

    public AudioClip CollisionClip;
    public AudioClip gameOverClip;
    public AudioClip coinClip;
    public AudioClip gamewinclip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        PlayBackgroundMusic(bgmusic); // starts the loop
    }

    
public void SetSFXEnabled(bool isEnabled)
{
    sfxSource.mute = !isEnabled;
}

public void SetMusicEnabled(bool isEnabled)
{
    backgroundMusicSource.mute = !isEnabled;
}
        

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (clip != null)
        {
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    // Convenience methods
    public void PlayCollideSFX() => PlaySFX(CollisionClip);
    public void PlayGameOverSFX() => PlaySFX(gameOverClip);
    public void PlayCoinSFX() => PlaySFX(coinClip);
    public void PlayGameWonSFX() => PlaySFX(gamewinclip);
}
