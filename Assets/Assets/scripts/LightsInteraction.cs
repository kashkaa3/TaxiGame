using UnityEngine;

public class LightsInteraction : MonoBehaviour
{
    public InteractiveViewManager viewManager;
    public CursorSystem cursorV;

    private bool lightsOpened = false;

    void Start()
    {
        cursorV.OnExitUIMode += CloseLights;
    }

    void OnDestroy()
    {
        if(cursorV != null)
        {
            cursorV.OnExitUIMode -= CloseLights;
        }
    }

    public void Interact()
    {
        Debug.Log("LightsInteraction.Interact called");

        if(viewManager == null)
        {
            Debug.LogError("LightsInteraction: View manager missing in inspector");
            return;
        }

        viewManager.ToggleLights();
        cursorV.InUIMode();
        lightsOpened = true;
    }

    private void CloseLights()
    {
        if (!lightsOpened) return;

        viewManager.HideAll();
        lightsOpened = false;

        Debug.Log("Lights closed");
    }


}
