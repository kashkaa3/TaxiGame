using UnityEngine;
using System.Collections;
using UnityEngine.Localization.Settings;
using TMPro;

public class LanguageSettings : MonoBehaviour
{
    public TMP_Text label;
    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        SetLanguage(PlayerPrefs.GetString("Language", "en"));
    }

    public void Toggle()
    {
        string currentLanguage = PlayerPrefs.GetString("Language", "en");
        SetLanguage(currentLanguage == "en" ? "ko" : "en");
    }

    void SetLanguage (string code)
    {
        PlayerPrefs.SetString("Language", code);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(code);

        label.text = code == "en" ? "English" : "한국어";
    }
}
