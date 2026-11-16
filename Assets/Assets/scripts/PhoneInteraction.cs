using System.Collections;
using TMPro;
using UnityEngine;

public class PhoneInteraction : MonoBehaviour
{
    public TextMeshProUGUI warningText;

    //public GameObject Background;
    public GameObject PhoneImage;

    public CursorSystem cursorV;

    private bool phoneOpened = false;
    private void Awake()
    {
        warningText.gameObject.SetActive(false);
        //Background.SetActive(false);
        PhoneImage.SetActive(false);
    }

    void Start()
    {
        cursorV.OnExitUIMode += ClosePhone; // Subscribe to the event
    }

    public void Interact()
    {
        if (GameStateManager.Instance.IsDriving)
        {
            StartCoroutine(ShowWarning("Warning_PhoneDriving"));
            Debug.Log("You can't use your phone while driving");
            return;
        }
        if (!GameStateManager.Instance.RideFinished)
        {
            StartCoroutine(ShowWarning("Warning_PhonePassenger"));
            Debug.Log("Finish ride");
            return;
        }

        OpenPhone();

        cursorV.InUIMode();

    }

    IEnumerator ShowWarning (string key)
    {
        warningText.text = DialogueLoc.GetUItext(key);
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);//WaitForSecondsRealtime use when world stops

        warningText.gameObject.SetActive(false);
    }

    void OpenPhone()
    {
        //Background.SetActive(true);
        PhoneImage.SetActive(true);
        Debug.Log("Phone Opened");
        phoneOpened = true;

        //Time.timeScale = 0f; //when phone is opened world stops

    }

    void ClosePhone()
    {
        if (!phoneOpened) return; // Prevent closing if the phone is not open
        //Background.SetActive(false);
        PhoneImage.SetActive(false);
        Debug.Log("Phone Closed");
        phoneOpened = false;
        //Time.timeScale = 1f; //when phone is closed world continues
    }

}
