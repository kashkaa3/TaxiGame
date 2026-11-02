using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PanelFader : MonoBehaviour
{
    [Header("Time (Seconds")]
    [SerializeField] private float fadeInDuration = 0.1f;
    [SerializeField] private float fadeOutDuration = 0.15f;

    private CanvasGroup canvasGroup;
    private Coroutine currentFade;

    private CanvasGroup Group
    {
        get
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            return canvasGroup;
        }
    }

    private void OnEnable()
    {
        Group.alpha = 0f;
        Group.interactable = false;
        Group.blocksRaycasts = false;
        StartFade(1f, fadeInDuration, deactivateAtEnd: false);
    }

    private void OnDisable()
    {
        currentFade = null;
    }

    public void Show()
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            StartFade(1f, fadeInDuration, deactivateAtEnd: false);
    }

    public void Hide()
    {
        //if it´s already deactivaed, no fade
        if (!gameObject.activeSelf) return;
        StartFade(0f, fadeOutDuration, deactivateAtEnd: true);
    }

    public void HideInstant()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        Group.alpha = 0f;
        gameObject.SetActive(false);
    }

    private void StartFade(float targetAlpha, float duration, bool deactivateAtEnd)
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeRoutine(targetAlpha, duration, deactivateAtEnd));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, bool deactivateAtEnd)
    {
        //while transition, it can´t interact
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; //doesn´t depend on Time.timeScale
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        Group.alpha = targetAlpha;

        if (deactivateAtEnd)
        {
            gameObject.SetActive(false);
        }
        else
        {
            //Only when it´s visible you can interact
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        currentFade = null;
    }
}
