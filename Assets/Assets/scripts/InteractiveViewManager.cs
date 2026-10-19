using UnityEngine;

public class InteractiveViewManager : MonoBehaviour
{
    [Header("Views (InteractivePanel sons)")]
    public GameObject gpsView;
    public GameObject radioView;
    public GameObject lightsView;

    [Header("Cameras")]
    public Camera minimapCamera;

    private bool gpsOpen = false;
    private bool radioOpen = false;
    private bool lightsOpen = false;

    void Awake()
    {
        HideAll();
    }

    //---GPS---

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

    //---RADIO---

    public void ToggleRadio()
    {
        if (radioOpen)
        {
            HideAll();
        }
            
        else
        {
            ShowRadio();
        }
    }

    public void ShowRadio()
    {
        HideAll();
        radioView.SetActive(true);
        radioOpen = true;
    }

    //---LIGHTS---

    public void ToggleLights()
    {
        if (lightsOpen) HideAll();
        else ShowLights();
    }

    public void ShowLights()
    {
        HideAll();
        lightsView.SetActive(true);
        lightsOpen = true;
    }

    //---COMMON---

    public void HideAll()
    {
        gpsView.SetActive(false);
        radioView.SetActive(false);
        lightsView.SetActive(false);
        minimapCamera.enabled = false;
        gpsOpen = false;
        radioOpen = false;
        lightsOpen = false;
    }
}
