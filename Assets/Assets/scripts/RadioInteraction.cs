using UnityEngine;

public class RadioInteraction : MonoBehaviour
{
    public InteractiveViewManager viewManager;
    public CursorSystem cursorV;
    public RadioController radio;

    private bool radioOpened = false;

    void Start()
    {
        cursorV.OnExitUIMode += CloseRadio;
    }

    void OnDestroy()
    {
        if (cursorV != null)
        {
            cursorV.OnExitUIMode -= CloseRadio;
        }
    }

    public void Interact()
    {
        Debug.Log("RadioInteraction.Interact called");

        if (viewManager == null)
        {
            Debug.LogError("RadioInteraction: View manager missing in inspector");
            return;
        }

        if (radio != null) radio.TurnOn();
        else Debug.LogWarning("RadioInteraction: RAdio missing in inspector");

        viewManager.ToggleRadio();
        cursorV.InUIMode();
        radioOpened = true;
    }

    private void CloseRadio()
    {
        if (!radioOpened) return;

        viewManager.HideAll();
        radioOpened= false;

        Debug.Log("Radio Closed");
    }
}
