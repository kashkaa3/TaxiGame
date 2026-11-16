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
            StartCoroutine(ShowWarning("Warning_SmokeDriving"));
            return;
        }
        if (!GameStateManager.Instance.RideFinished)
        {
            StartCoroutine(ShowWarning("Warning_SmokePassenger"));
            return;
        }
        Smoke();
    }

    IEnumerator ShowWarning(string key)
    {
        warningText.text = DialogueLoc.GetUItext(key);
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);//WaitForSecondsRealtime use when world stops
        warningText.gameObject.SetActive(false);
    }

    IEnumerator ShowSaving(string key)
    {
        var op = DialogueLoc.GetUI(key);
        yield return op;
        savingText.text = op.Result;
        savingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);//WaitForSecondsRealtime use when world stops
        savingText.gameObject.SetActive(false);
        Background.SetActive(false);
    }

    void Smoke()
    {
        Background.SetActive(true);
        savingSystem.SaveGame();
        StartCoroutine(ShowSaving("Saving_Game"));
    }
}
