using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sensitivitySlider;

    [Header("Numbers")]
    public TMP_Text musicVolumeText;
    public TMP_Text soundVolumeText;
    public TMP_Text sensitivityText;

    void Start()
    {
        // Initialize sliders with current settings
        musicSlider.SetValueWithoutNotify(GameSettings.MusicLevel);
        soundSlider.SetValueWithoutNotify(GameSettings.SoundLevel);
        sensitivitySlider.SetValueWithoutNotify(GameSettings.SensLevel);

        // Update the text fields with current settings
        musicVolumeText.text = GameSettings.MusicLevel.ToString();
        soundVolumeText.text = GameSettings.SoundLevel.ToString();
        sensitivityText.text = GameSettings.SensLevel.ToString();

        // Add listeners to sliders to update settings when changed
        musicSlider.onValueChanged.AddListener(v =>
        {
            GameSettings.MusicLevel = (int)v;
            musicVolumeText.text = ((int)v).ToString();
        });

        soundSlider.onValueChanged.AddListener(v =>
        {
            GameSettings.SoundLevel = (int)v;
            soundVolumeText.text = ((int)v).ToString();
        });

        sensitivitySlider.onValueChanged.AddListener(v =>
        {
            GameSettings.SensLevel = (int)v;
            sensitivityText.text = ((int)v).ToString();
        });
    }

    void OnDisable()
    {
        GameSettings.Save(); // Save settings when the menu is closed
    }
}
