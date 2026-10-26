using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboData
{
    public string mood;
    public string song;
}

public class AtmosphereBridge : MonoBehaviour
{
    [Header("References")]
    public AtmosphereController atmosphere;
    public RadioController radio;
    public RGBBlipSystem lights;

    [Header("Radio")]
    [Tooltip("If true, a radio that is turned off sends no song at all")]
    public bool onlyCountIfRadioOn = false;

    [Header("Lights: mood id for each dominant color")]
    public string redMood = "angry";
    public string greenMood = "happy";
    public string blueMood = "sad";
    public string neutralMood = "neutral";

    [Header("Combos used by the test checkpoint")]
    public List<ComboData> testCombos = new List<ComboData>();

    public event System.Action<AtmosphereResult> OnResult;

    private string lastSong = null;
    private bool wasActive = false;
    

    void OnEnable()
    {
        if (lights != null) lights.OnBlipsChanged += SendMood;
    }

    void OnDisable()
    {
        if (lights != null) lights.OnBlipsChanged -= SendMood;
    }

    void Update()
    {
        if (radio != null)
        {
            string song = GetCurrentSongId();
            if (song != lastSong)
            {
                lastSong = song;
                atmosphere.SetSong(song);
            }
        }

        bool active = atmosphere.isInputActive;

        if (!wasActive && active)
        {
            SendMood();
            SendSong();
        }

        if (wasActive && !active)
        {
            OnResult?.Invoke(atmosphere.lastResult);
        }
        wasActive = active;
    }

    //---CHECKPOINT---

    //Call this from wherever the check must begin
    public void StartCheckpoint(List<ComboData> comboData)
    {
        List<Combo> combos = new List<Combo>();
        foreach (ComboData c in comboData)
            combos.Add(new Combo(c.mood, c.song));

        atmosphere.Checkpoint(combos);
        SendSong();
        SendMood();
    }

    public void StartCheckpoint()
    {
        StartCheckpoint(testCombos);
    }

    [ContextMenu("Test: StartCheckpoint")]
    void TestCheckpoint()
    {
        StartCheckpoint();
    }

    //--- SENDING VALUES ---

    void SendSong()
    {
        if (radio == null) return;
        lastSong = GetCurrentSongId();
        atmosphere.SetSong(lastSong);
    }

    void SendMood()
    {
        if (lights == null) return;
        atmosphere.SetMood(GetMoodId());
    }

    string GetCurrentSongId()
    {
        if (onlyCountIfRadioOn && !radio.IsOn) return "";

        int index = radio.CurrentStationIndex;

        if (index >= 0 && index < radio.stations.Length)
        {
            string stationName = radio.stations[index].name;
            if (!string.IsNullOrWhiteSpace(stationName)) return stationName;
        }

        return "station" + index; 
    }

    string GetMoodId()
    {
        BlipColor? dominant = lights.DominantColor;

        if (dominant == BlipColor.R) return redMood;
        if (dominant == BlipColor.G) return greenMood;
        if (dominant == BlipColor.B) return blueMood;
        return neutralMood;
    }
}
