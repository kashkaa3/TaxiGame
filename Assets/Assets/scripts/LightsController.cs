using System.Collections;
using UnityEngine;

public class LightsController : MonoBehaviour
{
    public RGBBlipSystem blipSystem;
    public Light[] lights;
    public float colorSmoothSpeed = 3f;

    private Color currentColor = Color.white;
    private float[] baseIntensity;
    private Coroutine flickerRoutine;

    void Awake()
    {
        baseIntensity = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
            baseIntensity[i] = lights[i].intensity;
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
        Color target = blipSystem.OutputColor;
        currentColor = Color.Lerp(currentColor, target, Time.deltaTime * colorSmoothSpeed);

        foreach (Light l in lights)
            l.color = currentColor;
    }

    private void StartFlicker(float duration)
    {
        if (flickerRoutine != null) StopCoroutine(flickerRoutine);
        flickerRoutine = StartCoroutine(FlickerRoutine(duration));
    }

    private IEnumerator FlickerRoutine(float duration)
    {
        for (float t = 0; t< duration; t+= 0.07f)
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
