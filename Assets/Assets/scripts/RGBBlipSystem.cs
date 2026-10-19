using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlipColor { R, G, B }

public class RGBBlipSystem : MonoBehaviour
{
    [Header("Blips")]
    public int maxBlips = 10;

    [Header("Glitch")]
    public float minGlitchInterval = 30f; //minimum seconds between glitches
    public float maxGlitchInterval = 90f;
    public float glitchDuration = 1f;     //glich duration
    public int minglitchBlips = 3;
    public int maxGlitchBlips = 6;

    private readonly List<BlipColor> blips = new List<BlipColor>();

    public IReadOnlyList<BlipColor> Blips => blips;

    public event System.Action OnBlipsChanged;
    public event System.Action<float> OnGlitchStarted;

    void Start()
    {
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

    private IEnumerator GlitchLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minGlitchInterval, maxGlitchInterval));

            OnGlitchStarted?.Invoke(glitchDuration);
            yield return new WaitForSeconds(glitchDuration);

            int amount = Random.Range(minglitchBlips, maxGlitchBlips + 1);
            for (int i = 0; i < amount; i++)
            {
                BlipColor random = (BlipColor)Random.Range(0, 3);
                AddBlip(random);
            }
        }

        [ContextMenu("Test: Add Red")]
        void TestRed()
        { AddRed(); }

        [ContextMenu("Test: Add Green")]
        void TestGreen()
        { AddGreen(); }

        [ContextMenu("Test: Add Blue")]
        void TestBlue()
        { AddBlue(); }


    
    }

}
