using System.Collections;
using UnityEngine;

[System.Serializable]
public class RadioStation
{
    public string name;
    public AudioClip clip;
}


public class RadioController : MonoBehaviour
{

    [Header("Audio")]
    public AudioSource audioSource;
    public RadioStation[] stations = new RadioStation[4];

    [Header("Fade")]
    public float fadeDuration = 0.5f;

    private int currentStation = 0;
    private bool isOn = false;
    private float userVolume = 1f;
    private Coroutine fadeRoutine;

    public bool IsOn => isOn;
    public int CurrentStation => currentStation;
    public string CurrentStationName => stations.Length > 0 ? stations[currentStation].name : "";

    //Tells other scripts that the station changed
    public event System.Action<int> OnStationChanged;

    void Start()
    {
        userVolume = audioSource.volume;
    }


    public void TogglePower()
    {
        isOn = !isOn;

        if (isOn) StartFade(stations[currentStation].clip, true);
        else StartFade(null, false);
    }

    public void NextStation()
    {
        ChangeStation((currentStation + 1) % stations.Length);
    }

    public void PreviousStation()
    {
        ChangeStation((currentStation - 1 + stations.Length) % stations.Length);
    }
    
    public void SelectStation(int index)
    {
        if (index < 0 || index >= stations.Length) return;
        ChangeStation(index);
    }

    public void SetVolume(float value)
    {
        userVolume = Mathf.Clamp01(value);

        if (fadeRoutine == null) audioSource.volume = userVolume;
    }

    private void ChangeStation(int newIndex)
    {
        if (stations.Length == 0 || newIndex == currentStation) return;

        currentStation = newIndex;
        OnStationChanged?.Invoke(currentStation);

        if (isOn) StartFade(stations[currentStation].clip, true);
    }

    private void StartFade(AudioClip newClip, bool playAfter)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeRoutine(newClip, playAfter));
    }

    private IEnumerator FadeRoutine(AudioClip newClip, bool playAfter)
    {
        float startVolume = audioSource.volume;
        for (float t= 0; t < fadeDuration; t+= Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;

        if (playAfter && newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            audioSource.volume = userVolume;
            fadeRoutine = null;
            yield break;
        }

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, userVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = userVolume;
        fadeRoutine = null;
    }
}
