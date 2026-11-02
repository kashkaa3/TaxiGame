using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Volume : MonoBehaviour
{
    public enum Channel
    {
        Music,
        Sound
    }
    public Channel channel = Channel.Sound;

    AudioSource audioSource;
    float volume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume;
    }

    void OnEnable()
    {
        GameSettings.OnSettingsChanged += Apply;
        Apply();
    }

    void OnDisable()
    {
        GameSettings.OnSettingsChanged -= Apply;
    }

    void Apply()
    {
        float settings = channel == Channel.Music ? GameSettings.MusicVolume : GameSettings.SoundVolume;
        audioSource.volume = volume * settings;
    }
}
