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
    public AudioSource audioSource; // music
    public AudioSource noiseSource; // white noise
    public AudioClip noiseClip; // if empty, one is generated
    public RadioStation[] stations = new RadioStation[4];

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float noiseVolume = 0.3f;
    public float powerFadeSpeed = 2;


    private int currentStation = 0;
    [SerializeField]private bool isOn = false;
    [SerializeField]private float signal = 1f;  // 0 = Noise ; 1 = Music
    [SerializeField]private float master = 0f;  // general volume

    public bool IsOn => isOn;
    public float Signal => signal;
    public int CurrentStationIndex => currentStation;
    public string CurrentStationName => stations.Length > 0 ? stations[currentStation].name : "";
    

    //Tells other scripts that the station changed
    public event System.Action<int> OnStationChanged;

    void Start()
    {
        noiseSource.clip = noiseClip != null ? noiseClip : GenerateNoise();
        noiseSource.loop = true;
        noiseSource.Play();

        PlayCurrentStation();
    }

    void Update()
    {
        float targetMaster = isOn ? 1f : 0f;
        master = Mathf.MoveTowards(master, targetMaster, powerFadeSpeed * Time.deltaTime);

        audioSource.volume = musicVolume * signal * master;
        noiseSource.volume = noiseVolume * (1f - signal) * master;
    }


    public void TurnOn()
    {
        isOn = true;
    }

    public void TogglePower()
    {
        isOn = !isOn;

    }

    public void SetSignal(float value)
    {
        signal = Mathf.Clamp01(value);
    }

    public void SetVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
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

 



    private void ChangeStation(int newIndex)
    {
        if (stations.Length == 0 || newIndex == currentStation) return;

        currentStation = newIndex;
        PlayCurrentStation();
        OnStationChanged?.Invoke(currentStation);
    }

    private void PlayCurrentStation()
    {
        if (stations.Length == 0 || stations[currentStation].clip == null)
        {
            audioSource.Stop();
            return;
        }

        audioSource.clip = stations[currentStation].clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private AudioClip GenerateNoise()
    {
        int sampleRate = 44100;
        float[] samples = new float[sampleRate * 2];

        for (int i = 0;  i < samples.Length; i++)
            samples[i] = Random.Range(-1f, 1f);

        AudioClip clip = AudioClip.Create("GeneratedNoise", samples.Length, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    
}
