using UnityEngine;
using System;

public static class GameSettings
{
    public static event Action OnSettingsChanged;

    public static int MusicLevel
    {
        get => PlayerPrefs.GetInt("MusicLevel", 100);
        set
        {
            PlayerPrefs.SetInt("MusicLevel", Mathf.Clamp(value, 0, 100));
            OnSettingsChanged?.Invoke();
        }
    }

    public static int SoundLevel { 
        get => PlayerPrefs.GetInt("SoundLevel", 100);
        set
        {
            PlayerPrefs.SetInt("SoundLevel", Mathf.Clamp(value, 0, 100));
            OnSettingsChanged?.Invoke();
        }
    }

    public static int SensLevel {

        get => PlayerPrefs.GetInt("SensLevel", 50); // Default sensitivity level is 50
        set
        {
            PlayerPrefs.SetInt("SensLevel", Mathf.Clamp(value, 1, 100));
            OnSettingsChanged?.Invoke();
        }
    }

    // Convenience properties to get the volume and sensitivity as floats between 0 and 1
    public static float MusicVolume => MusicLevel / 100f;
    public static float SoundVolume => SoundLevel / 100f;
    public static float Sensitivity => SensLevel / 100f;

    public static void Save() => PlayerPrefs.Save();

}
