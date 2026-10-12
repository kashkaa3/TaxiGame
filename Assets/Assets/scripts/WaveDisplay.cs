using UnityEngine;

public class WaveDisplay : MonoBehaviour
{
    [Header("Preview")]
    public bool previewMode = true; 
    [Range(0f,1f)]
    public float previewProgress = 1f;

    [Header("Connections")]
    public AtmosphereController atmosphere;
    public GameObject wavePanel;
    public GameObject dotTemplate;
    public GameObject dialogueText;

    [Header("Wave shape")]
    public int dotCount = 100;
    public float width = 700f;
    public float height = 40f;
    public float cycles = 2f;

    [Header("Distortion")]
    public float distortionPerfect = 0f;
    public float distortionClose = 0.3f;
    public float distortionOneCorrect = 0.6f;
    public float distortionNone = 1f;

    public float extraHight = 30f;
    public float extraCycles = 5f;
    public float wobbleHeight = 12f;
    public float wobbleSpeed = 6f;
    public float changeSpeed = 1.5f;

    RectTransform[] dots;
    float distortion = 1f;

    void Start()
    {
        dots = new RectTransform[dotCount];
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotTemplate, wavePanel.transform);
            dot.SetActive(true);
            dots[i] = dot.GetComponent<RectTransform>();
        }
        dotTemplate.SetActive(false);
        wavePanel.SetActive(false);
    }

    void Update()
    {
        bool showWave;
        float target;

        if (previewMode)
        {
            showWave = true;
            target = previewProgress;
        }
        else
        {
            showWave = atmosphere.isInputActive;
            target = GetDistortion(atmosphere.GetCurrentResult().level);
        }

        wavePanel.SetActive(showWave);
        if (dialogueText != null)
        {
            dialogueText.SetActive(!showWave);
        }
        if (!showWave)
        {
            distortion = 1f;
            return;
        }

        distortion = Mathf.MoveTowards(distortion, target, changeSpeed * Time.deltaTime);
        MoveDots();
    }

    float GetDistortion(FitLevel level)
    {
        if (level == FitLevel.Perfect) return distortionPerfect;
        if (level == FitLevel.Close) return distortionClose;
        if (level == FitLevel.OneCorrect) return distortionOneCorrect;
        return distortionNone;
    }

    void MoveDots()
    {
        float currentCycle = cycles + extraCycles * distortion;
        float currentHeight = height + extraHight * distortion;

        for (int i =0; i < dotCount; i++)
        {
            float progress = i / (float)(dotCount - 1);
            float x = (progress - 0.5f) * width;

            float y = Mathf.Sin(progress * currentCycle * Mathf.PI * 2f) * currentHeight;
            float wobble = Mathf.Sin(progress * 30f + Time.time * wobbleSpeed) * wobbleHeight * distortion;

            dots[i].anchoredPosition = new Vector2(x, y + wobble);
        }
    }
}
