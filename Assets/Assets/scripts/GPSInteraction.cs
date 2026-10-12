using UnityEngine;

public class GPSInteraction : MonoBehaviour
{
    public InteractiveViewManager viewManager;
    public CursorSystem cursorV;

    private bool gpsOpened = false;

    void Start()
    {
        cursorV.OnExitUIMode += CloseGPS; // Subscribe to the event
    }

    public void Interact()
    {
        Debug.Log("GPSInteraction.Interact called");

        if (viewManager == null)
        {
            Debug.LogError("GPSInteraction: View manger missing in inspector");
            return;
        }
        
        viewManager.ToggleGPS();
        cursorV.InUIMode();
        gpsOpened = true;
    }

    private void CloseGPS()
    {
        if (!gpsOpened) return;

        viewManager.HideAll();
        gpsOpened = false;
        
        Debug.Log("GPS closed");
    }
}
