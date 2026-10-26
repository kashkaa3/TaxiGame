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
    [Tooltip("Below this signal (static) the song doesn´t count")]
    [Range(0.1f, 1f)] public float minSignal = 0.5f;

    //TO DO REAL EMOTIONS NAMES
    [Header("Lights: mood id for each dominant color")]
    public string redMood = "red";
    public string greenMood = "green";
    public string blueMood = "blue";
    public string redGreenMood = "redgreen";
    public string redBlueMood = "redblue";
    public string greenBlueMood = "greenblue";

    [Header("Debug")]
    public bool debugLog = false;

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
                if (debugLog) Debug.Log("Bridge -> song: '" + song + "'");
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
        if (debugLog) Debug.Log("Bridge -> song: '" + lastSong + "'");
    }

    void SendMood()
    {
        if (lights == null) return;
        string mood = GetMoodId();
        atmosphere.SetMood(mood);
        if (debugLog) Debug.Log("Bridge -> mood: '" + mood + "'");
    }

    string GetCurrentSongId()
    {
        if (!radio.IsOn) return ""; // radio off
        if (radio.Signal < minSignal) return ""; // too much static

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
        switch (lights.CurrentBlend)
        {
            case ColorBlend.Red:        return redMood;
            case ColorBlend.Green:      return greenMood;
            case ColorBlend.Blue:       return blueMood;
            case ColorBlend.RedBlue:    return redBlueMood;
            case ColorBlend.RedGreen:   return redGreenMood;
            case ColorBlend.GreenBlue:  return greenBlueMood;
            default:                    return "";
            
        }
    }
}
