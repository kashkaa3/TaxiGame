using UnityEngine;
using UnityEngine.UI;

public class LightsUI : MonoBehaviour
{
    public RGBBlipSystem blipSystem;
    public Image[] slots;

    public Color emptyColor = new Color(1f, 1f, 1f, 0.15f);
    public Color redColor = Color.red;
    public Color greenColor = Color.green;
    public Color blueColor = Color.blue;

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
                slots[i].color = emptyColor;
            else
                slots[i].color = ColorFor(blips[i - offset]);
        }
    }

    private Color ColorFor(BlipColor blip)
    {
        switch (blip)
        {
            case BlipColor.R: return redColor;
            case BlipColor.G: return greenColor;
            default:          return blueColor;
        }
    }
}
