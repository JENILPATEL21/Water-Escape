using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public enum SoundType { Music, SFX }
    public SoundType soundType;

    public Image toggleImage;
    public Sprite onSprite;
    public Sprite offSprite;
    private bool isOn;

    private void Start()
    {
        // Set initial state based on current audio settings
        if (soundType == SoundType.Music)
        {
            isOn = !AudioManager.Instance.backgroundMusicSource.mute; // Use AudioManager to check current state
        }
        else if (soundType == SoundType.SFX)
        {
            isOn = !AudioManager.Instance.sfxSource.mute; // Use AudioManager to check current state
        }

        UpdateAudio();
        UpdateSprite();
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateAudio();
        UpdateSprite();
    }

    void UpdateAudio()
    {
        if (soundType == SoundType.Music)
        {
            AudioManager.Instance.SetMusicEnabled(isOn);
        }
        else if (soundType == SoundType.SFX)
        {
            AudioManager.Instance.SetSFXEnabled(isOn);
        }
    }

    void UpdateSprite()
    {
        toggleImage.sprite = isOn ? onSprite : offSprite;
    }
}
