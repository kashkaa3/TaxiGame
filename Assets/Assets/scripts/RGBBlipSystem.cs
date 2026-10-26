using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlipColor { R, G, B }

public enum ColorBlend { Nothing, Red, Green, Blue, RedGreen, RedBlue, GreenBlue }

public class RGBBlipSystem : MonoBehaviour
{
    [Header("Blips")]
    public int maxBlips = 10;

    [Header("Story emotion")]
    [Range(0.5f, 1f)] public float minShare = 0.3f;

    [Header("Glitch")]
    public bool glitchesEnabled = false;   // Desactivados para la beta
    public float minGlitchInterval = 30f;  // minimum seconds between glitches
    public float maxGlitchInterval = 90f;
    public float glitchDuration = 1f;      // glitch duration
    public int minGlitchBlips = 2;
    public int maxGlitchBlips = 5;

    private readonly List<BlipColor> blips = new List<BlipColor>();

    public IReadOnlyList<BlipColor> Blips => blips;

    public event System.Action OnBlipsChanged;
    public event System.Action<float> OnGlitchStarted;

    void Start()
    {
        if (glitchesEnabled)
            StartCoroutine(GlitchLoop());
    }

    //UI Buttons methods
    public void AddRed()    { AddBlip(BlipColor.R); }
    public void AddGreen()  { AddBlip(BlipColor.G); }
    public void AddBlue()   { AddBlip(BlipColor.B); }

    public void AddBlip(BlipColor color)
    {
        //If it´s full, the oldest is pushed out
        if (blips.Count >= maxBlips) blips.RemoveAt(0);

        blips.Add(color);
        OnBlipsChanged?.Invoke();

        Debug.Log("Blip added: " + color + " ( " + blips.Count + "/" + maxBlips + ")");
    }

    public int CountOf(BlipColor color)
    {
        int count = 0;
        foreach (BlipColor b in blips)
            if (b == color) count++;
        return count;
    }

    //final colour of the light
    public Color OutputColor
    {
        get
        {
            if (blips.Count == 0) return Color.white;

            float r = CountOf(BlipColor.R) / (float)blips.Count;
            float g = CountOf(BlipColor.G) / (float)blips.Count;
            float b = CountOf(BlipColor.B) / (float)blips.Count;

            return new Color(r, g, b);
        }
    }

    //The color with more blips
    public BlipColor? DominantColor
    {
        get
        {
            int r = CountOf(BlipColor.R);
            int g = CountOf(BlipColor.G);
            int b = CountOf(BlipColor.B);

            if (r == 0 && g == 0 && b == 0) return null;
            if (r > g && r > b) return BlipColor.R;
            if (g > r && g > b) return BlipColor.G;
            if (b > r && b > g) return BlipColor.B;
            return null;
        }
    }

    public ColorBlend CurrentBlend
    {
        get
        {
            int total = blips.Count;
            if (total == 0) return ColorBlend.Nothing;

            float margin = 0.0001f; // no rounding
            bool hasR = CountOf(BlipColor.R) / (float)total >= minShare - margin;
            bool hasG = CountOf(BlipColor.G) / (float)total >= minShare - margin;
            bool hasB = CountOf(BlipColor.B) / (float)total >= minShare - margin;

            //CASES
            if (hasR && hasG && hasB) return ColorBlend.Nothing;
            if (hasR && hasG) return ColorBlend.RedGreen;
            if (hasR && hasB) return ColorBlend.RedBlue;
            if (hasG && hasB) return ColorBlend.GreenBlue;
            if (hasR) return ColorBlend.Red;
            if (hasG) return ColorBlend.Green;
            if (hasB) return ColorBlend.Blue;

            return ColorBlend.Nothing;
        }
    }

    private IEnumerator GlitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minGlitchInterval, maxGlitchInterval));

            OnGlitchStarted?.Invoke(glitchDuration);
            yield return new WaitForSeconds(glitchDuration);

            int amount = Random.Range(minGlitchBlips, maxGlitchBlips + 1);
            for (int i = 0; i < amount; i++)
            {
                BlipColor random = (BlipColor)Random.Range(0, 3);
                AddBlip(random);
            }
        }
    }

}
