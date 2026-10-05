using System.Collections;
using TMPro;
using UnityEngine;

public class CigaretteInteraction : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI savingText;

    public GameObject Background;

    public SavingSystem savingSystem;

    private void Awake()
    {
        warningText.gameObject.SetActive(false);
        savingText.gameObject.SetActive(false);
        Background.SetActive(false);
    }

    public void Interact()
    {
        if (GameStateManager.Instance.IsDriving) { 
            StartCoroutine(ShowWarning("You can't smoke while driving"));
            return;
        }
        if (!GameStateManager.Instance.RideFinished)
        {
            StartCoroutine(ShowWarning("It's rude to smoke with passenger inside"));
            return;
        }
        Smoke();
    }

    IEnumerator ShowWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);//WaitForSecondsRealtime use when world stops
        warningText.gameObject.SetActive(false);
    }

    IEnumerator ShowSaving(string message)
    {
        savingText.text = message;
        savingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);//WaitForSecondsRealtime use when world stops
        savingText.gameObject.SetActive(false);
        Background.SetActive(false);
    }

    void Smoke()
    {
        Background.SetActive(true);
        savingSystem.SaveGame();
        StartCoroutine(ShowSaving("Game is saving..."));
    }
}
