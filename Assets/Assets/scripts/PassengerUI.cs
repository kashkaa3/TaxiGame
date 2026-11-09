using System;
using UnityEngine;

public class PassengerUI : MonoBehaviour
{
    public static PassengerUI Instance;

    public GameObject passengerPanel;
    [SerializeField] private Animator slideAnimator;

    private static readonly int EnterState = Animator.StringToHash("PassengerEnter");

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPassenger()
    {
        passengerPanel.SetActive(true);
        PlayEnter();
    }

    public void HidePassenger()
    {
        PanelFader fader = passengerPanel.GetComponent<PanelFader>();
        if (fader != null) fader.Hide();
        else passengerPanel.SetActive(false);
    }

    public void TogglePassenger()
    {
        if (passengerPanel.activeSelf) HidePassenger();
        else ShowPassenger();
    }

    public void PlayEnter()
    {
        if (slideAnimator != null && slideAnimator.gameObject.activeInHierarchy)
            slideAnimator.Play(EnterState, 0, 0f);
    }

}
