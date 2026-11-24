using UnityEngine;

public class WaveDisplay : MonoBehaviour
{
    [Header("Guide wave")]
    public bool showGuideWave = true; // Whether to show the guide wave or not
    [Range(0f, 1f)] public float guideWaveAlpha = 0.25f; // Alpha value for the guide wave
    public float guideDotsSize = 1.6f; // Size of the guide dots

    [Header("Connections")]
    public AtmosphereController atmosphere;
    public GameObject wavePanel;
    public GameObject dotTemplate;
    public GameObject dialogueText;

    [Header("Wave shape")]
    public int dotCount = 200;
    public float width = 1000f;
    public float height = 25f;
    public float cycles = 3f;

    [Header("Distortion")]
    public float distortionPerfect = 0f;
    public float distortionClose = 0.2f; //answer when 2 options are right but not from the same combo
    public float distortionOneCorrect = 0.7f; //answer when 1 option is right but 2 are wrong
    public float distortionNone = 1f;

    [Header ("Mismatch effects")]
    public float breakHeight = 25f;
    public float breakWidth = 120f;

    public float extraHight = 50f;
    public float extraCycles = 8f;
    public float wobbleHeight = 15f;
    public float wobbleSpeed = 6f;
    public float waveSpeed = 2f;
    public float changeSpeed = 1.5f;

    RectTransform[] guideDots;
    RectTransform[] dots;
    float distortionHeight = 1f; //how mood wrong is
    float distortionCycles = 1f; //how song wrong is
    float mismatch = 0f; //0 = perfect, 1 = mismatch

    void Start()
    {
        //Create guide wave dots
        guideDots = new RectTransform[dotCount];
        for (int i = 0; i < dotCount; i++)
        {
            GameObject g = Instantiate(dotTemplate, wavePanel.transform);
            g.SetActive(showGuideWave);

            CanvasGroup cg = g.AddComponent<CanvasGroup>(); // Add a CanvasGroup component to control the alpha
            cg.alpha = guideWaveAlpha; // Set the alpha value for the guide wave

            guideDots[i] = g.GetComponent<RectTransform>();
            guideDots[i].localScale = Vector3.one * guideDotsSize; // Set the size of the guide dots
        }

        //Create game wave dots
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
            distortionHeight = 1f;
            distortionCycles = 1f;
            mismatch = 0f;
            return;
        }

        AtmosphereResult result = atmosphere.GetCurrentResult();
        float targetHeight = GetDistortion(result.level, result.moodCorrect);
        float targetCycles = GetDistortion(result.level, result.songCorrect);
        float targetMismatch = (result.level == FitLevel.Close) ? 1f : 0f; // breaks when close, otherwise no mismatch

        distortionHeight = Mathf.MoveTowards(distortionHeight, targetHeight, changeSpeed * Time.deltaTime);
        distortionCycles = Mathf.MoveTowards(distortionCycles, targetCycles, changeSpeed * Time.deltaTime);
        mismatch = Mathf.MoveTowards(mismatch, targetMismatch, changeSpeed * Time.deltaTime);

        GuideWave();
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
            float x = (progress - 0.5f) * (width - breakWidth * mismatch); //to not make wave go outside the panel

            float y = Mathf.Sin(progress * currentCycle * Mathf.PI * 2f + Time.time * waveSpeed) * currentHeight;

            float wobbleAmount = Mathf.Max(distortionHeight, distortionCycles);
            float wobble = Mathf.Sin(progress * 30f + Time.time * wobbleSpeed) * wobbleHeight * wobbleAmount;

            float step = (progress > 0.5f) ? breakHeight * mismatch : 0f; //the right half jumps up when it is broken
            float gap = ((progress > 0.5f) ? 1f : -1f) * breakWidth * 0.5f * mismatch; //left half moves to left, right to right

            dots[i].anchoredPosition = new Vector2(x + gap, y + wobble + step);
        }
    }

    void GuideWave()
    {
        for (int i = 0; i < dotCount; i++)
        {
            float progress = i / (float)(dotCount - 1); // Calculate the progress along the wave (0 to 1)
            float x = (progress - 0.5f) * width; 
            // Use the base height and cycles for the guide wave, without distortion
            float y = Mathf.Sin(progress * cycles * Mathf.PI * 2f + Time.time * waveSpeed) * height;
            guideDots[i].anchoredPosition = new Vector2(x, y);
        }
    }
}
