using System.Collections;
using UnityEngine;

public class LightsController : MonoBehaviour 
{
    public RGBBlipSystem blipSystem;
    public Light[] lights;
    public float colorSmoothSpeed = 3f;


    private Color[] startColor;
    private Color[] currentColor;
    private float[] baseIntensity;
    private Coroutine flickerRoutine;

    void Awake()
    {
        baseIntensity = new float[lights.Length];
        startColor = new Color[lights.Length];
        currentColor = new Color[lights.Length];

        for (int i = 0; i < lights.Length; i++)
        {
            baseIntensity[i] = lights[i].intensity;
            startColor[i] = lights[i].color;
            currentColor[i] = lights[i].color;
        }
            
    }

    void OnEnable()
    {
        blipSystem.OnGlitchStarted += StartFlicker;
    }
    void OnDisable()
    {
        blipSystem.OnGlitchStarted -= StartFlicker;
    }

    void Update()
    {
        bool noBlips = blipSystem.Blips.Count == 0;
        Color output = blipSystem.OutputColor;

        for (int i = 0; i < lights.Length; i++)
        {
            Color target = noBlips ? startColor[i] : output;

            currentColor[i] = Color.Lerp(currentColor[i], target, Time.deltaTime * colorSmoothSpeed);
            lights[i].color = currentColor[i];
        }
    }

    private void StartFlicker(float duration)
    {
        if (flickerRoutine != null) StopCoroutine(flickerRoutine);
        flickerRoutine = StartCoroutine(FlickerRoutine(duration));
    }

    private IEnumerator FlickerRoutine(float duration)
    {
        for (float t = 0; t < duration; t += 0.07f)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity = baseIntensity[i] * Random.Range(0f, 1f);
            }
            yield return new WaitForSeconds(0.07f);
        }

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = baseIntensity[i];
        }

        flickerRoutine = null;
    }

}