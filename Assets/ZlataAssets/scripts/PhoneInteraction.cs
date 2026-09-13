using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PhoneInteraction : MonoBehaviour
{
    public TextMeshProUGUI warnignText;

    public GameObject backgroung;
    public GameObject phoneImage;

    private bool phoneOpened = false;

    private void Awake()
    {
        warnignText.gameObject.SetActive(false);
        backgroung.SetActive(false);
        phoneImage.SetActive(false);
    }

    private void Update()
    {
        if (phoneOpened && Input.GetKeyDown(KeyCode.Q))
        {
            ClosePhone();
        }
    }

    public void Interact()
    {
        if (GameStateManager.Instance.IsDriving)
        {
            StartCoroutine(ShowWarning("You can't use your phone while driving"));
            return;
        }

        if (!GameStateManager.Instance.RideFinished)
        {
            StartCoroutine(ShowWarning("You can't use your phone while the ride is not finished"));
            return;
        }
        OpenPhone();

    }

    IEnumerator ShowWarning(string message)
    {
        warnignText.text = message;
        warnignText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f); //WaitForSecondsRealtime(3f) use when world stops 
        warnignText.gameObject.SetActive(false);
    }

    void OpenPhone() {
        backgroung.SetActive(true);
        phoneImage.SetActive(true);
        Debug.Log("Phone opened");
        phoneOpened = true;

        Time.timeScale = 0f; // Pause the game 
    }

    void ClosePhone()
    {
        backgroung.SetActive(false);
        phoneImage.SetActive(false);
        Debug.Log("Phone closed");
        phoneOpened = false;

        Time.timeScale = 1f; // Resume the game
    }
}
