using UnityEngine;
using UnityEngine.UI;

public class LightsUI : MonoBehaviour
{
    public RGBBlipSystem blipSystem;
    public Image[] slots;

    [Header("Blip Sprites")]
    public Sprite redBlip;
    public Sprite greenBlip;
    public Sprite blueBlip;

    void OnEnable()
    {
        blipSystem.OnBlipsChanged += Refresh;
        Refresh(); // When opening the panel, be up to date
    }

    void OnDisable()
    {
        blipSystem.OnBlipsChanged -= Refresh;
    }

    private void Refresh()
    {
        var blips = blipSystem.Blips;
        int offset = slots.Length - blips.Count;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < offset)
            {
                slots[i].enabled = false;
            }
            else
            {
                slots[i].enabled = true;
                slots[i].sprite = SpriteFor(blips[i - offset]);
                slots[i].color = Color.white;
            }
                
        }
    }

    private Sprite SpriteFor(BlipColor blip)
    {
        switch (blip)
        {
            case BlipColor.R: return redBlip;
            case BlipColor.G: return greenBlip;
            default:          return blueBlip;
        }
    }
}
