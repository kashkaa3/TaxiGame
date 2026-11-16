using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    
    public static bool IsShowing {  get; private set; }

    [Header("UI")]
    public CanvasGroup endGroup;    // Canvas Group 
    public Button menuButton;       // Button to go back to the main menu

    [Header("Setting")]
    public float fadeDuration = 1.5f;
    public string sceneName = "MenuScene";

    private float originalMasterVolume = 1f;
    private bool volumeChanged = false;

    private void Awake()
    {
        IsShowing = false;

        endGroup.alpha = 0f;
        endGroup.blocksRaycasts = false;
        endGroup.gameObject.SetActive(false);

        menuButton.onClick.AddListener(GoToMenu);
    }

    private void OnDestroy()
    {
        if (volumeChanged)
        {
            AudioListener.volume = originalMasterVolume;
        }
    }
    public void ShowEnd()
    {

        if (IsShowing) return; //avoid starting it twice
        IsShowing = true;
        StartCoroutine(EndRoutine());
    }

    private IEnumerator EndRoutine()
    {
        endGroup.gameObject.SetActive(true);
        endGroup.alpha = 0f;
        endGroup.blocksRaycasts = true; // blocks clicks

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        originalMasterVolume = AudioListener.volume;
        volumeChanged = true;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);

            endGroup.alpha = t;

            AudioListener.volume = Mathf.Lerp(originalMasterVolume, 0f, t);

            yield return null;
        }

        endGroup.alpha = 1f;

        AudioListener.volume = 0f;
        
        Time.timeScale = 0f; //freeze the game

    }

    private void GoToMenu()
    {
        Time.timeScale = 1f;
        IsShowing = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameSettings.Save();
        SceneManager.LoadScene(sceneName);
    }


}
