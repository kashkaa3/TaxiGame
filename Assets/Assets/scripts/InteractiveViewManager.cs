using UnityEngine;

public class InteractiveViewManager : MonoBehaviour
{
    [Header("Views (InteractivePanel sons)")]
    public GameObject gpsView;

    [Header("Cameras")]
    public Camera minimapCamera;

    private bool gpsOpen = false;

    void Awake()
    {
        HideAll();
    }

    public void ToggleGPS()
    {
        if (gpsOpen)
            HideAll();
        else
            ShowGPS();
    }

    public void ShowGPS()
    {
        HideAll();
        gpsView.SetActive(true);
        minimapCamera.enabled = true;
        gpsOpen = true;
    }

    public void HideAll()
    {
        gpsView.SetActive(false);
        minimapCamera.enabled = false;
        gpsOpen = false;
    }
}
