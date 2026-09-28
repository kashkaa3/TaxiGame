using UnityEngine;

public class PassengerUI : MonoBehaviour
{
    public static PassengerUI Instance;

    public GameObject passengerPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPassenger()
    {
        passengerPanel.SetActive(true);
    }

    public void HidePassenger()
    {
        passengerPanel.SetActive(false);
    }

    public void TogglePassenger()
    {
       

        passengerPanel.SetActive(!passengerPanel.activeSelf);
    }

}
