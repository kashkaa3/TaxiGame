using UnityEngine;

public class WaveDisplay : MonoBehaviour
{

    [Header("Connections")]
    public AtmosphereController atmosphere;
    public GameObject wavePanel;
    public GameObject dotTemplate;
    public GameObject dialogueText;

    [Header("Wave shape")]
    public int dotCount = 200;
    public float width = 1000f;
    public float height = 30f;
    public float cycles = 3f;

    [Header("Distortion")]
    public float distortionPerfect = 0f;
    public float distortionClose = 0.3f;
    public float distortionOneCorrect = 0.6f;
    public float distortionNone = 1f;

    public float extraHight = 30f;
    public float extraCycles = 5f;
    public float wobbleHeight = 12f;
    public float wobbleSpeed = 6f;
    public float waveSpeed = 2f;
    public float changeSpeed = 1.5f;

    RectTransform[] dots;
    float distortion = 1f; //for wobble
    float distortionHeight = 1f; //how mood wrong is
    float distortionCycles = 1f; //how song wrong is

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
        bool showWave = atmosphere.isInputActive;

        wavePanel.SetActive(showWave);
        if (dialogueText != null)
        {
            dialogueText.SetActive(!showWave);
        }
        if (!showWave)
        {
            distortion = 1f;
            distortionHeight = 1f;
            distortionCycles = 1f;
            return;
        }

        AtmosphereResult result = atmosphere.GetCurrentResult();
        float targetHeight = GetDistortion(result.level, result.moodCorrect);
        float targetCycles = GetDistortion(result.level, result.songCorrect);

        distortionHeight = Mathf.MoveTowards(distortionHeight, targetHeight, changeSpeed * Time.deltaTime);
        distortionCycles = Mathf.MoveTowards(distortionCycles, targetCycles, changeSpeed * Time.deltaTime);
        distortion = Mathf.Max(distortionHeight, distortionCycles);
        MoveDots();
    }

    float GetDistortion(FitLevel level, bool correct)
    {
        if (level == FitLevel.Perfect) return distortionPerfect;
        if (level == FitLevel.Close) return distortionClose;
        if (correct) return distortionPerfect; // If the level is not perfect or close, but the answer is correct, use perfect distortion
        if (level == FitLevel.OneCorrect) return distortionOneCorrect;
        return distortionNone;
    }

    void MoveDots()
    {

        float currentCycle = cycles + extraCycles * distortionCycles;
        float currentHeight = height + extraHight * distortionHeight;

        for (int i =0; i < dotCount; i++)
        {
            float progress = i / (float)(dotCount - 1);
            float x = (progress - 0.5f) * width;

            float y = Mathf.Sin(progress * currentCycle * Mathf.PI * 2f + Time.time * waveSpeed) * currentHeight;
            float wobble = Mathf.Sin(progress * 30f + Time.time * wobbleSpeed) * wobbleHeight * distortion;

            dots[i].anchoredPosition = new Vector2(x, y + wobble);
        }
    }
}
