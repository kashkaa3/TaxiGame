using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingOverlay : MonoBehaviour
{

    [Header("UI")]
    public CanvasGroup backgroundGroup;       // LoadingBackground (The image)
    public TextMeshProUGUI loadingText; // "loading.." text

    [Header("Audio")]
    public AudioSource drivingSound; //Ambience sound

    [Header("Settings")]
    public float loadingTime = 3f;
    public float fadeDuration = 1f;
    public float dotInterval = 0.4f;

    private float targetVolume = 1f;

    private void Awake()
    {
        //Show the loading screen and freeze the game 
        backgroundGroup.gameObject.SetActive(true);
        backgroundGroup.alpha = 1f;
        backgroundGroup.blocksRaycasts = true; // blocks clicks on things
        Time.timeScale = 0f;

        if (drivingSound != null)
        {
            targetVolume = drivingSound.volume;
            drivingSound.volume = 0f;
        }
    }

    private IEnumerator Start()
    {
        float elapsed = 0f;
        float dotTimer = 0f;
        int dotCount = 0;

        while (elapsed < loadingTime)
        {
            elapsed += Time.unscaledDeltaTime;
            dotTimer += Time.unscaledDeltaTime;

            if (dotTimer >= dotInterval)
            {
                dotTimer = 0f;
                dotCount = (dotCount + 1) % 4; // 0, 1, 2, 3 dots
                loadingText.text = "Loading" + new string('.', dotCount);
            }

            yield return null;
        }

        //The game starts now
        Time.timeScale = 1f;
        backgroundGroup.blocksRaycasts = false;

        if (drivingSound != null)
        {
            drivingSound.Play();
        }

        //Fade out image and fade in the sound
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);

            backgroundGroup.alpha = 1f - t;

            if (drivingSound != null)
            {
                drivingSound.volume = Mathf.Lerp(0f, targetVolume, t);
            }

            yield return null;
        }

        backgroundGroup.alpha = 0f;
        backgroundGroup.gameObject.SetActive(false);
    }
}
