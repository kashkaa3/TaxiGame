using UnityEngine;

public class RadioTuner : MonoBehaviour
{
    public RadioController radio;
    public CursorSystem cursorSystem;

    [Header("Wheel")]
    public RectTransform wheel;
    public float wheelStartAngle = 90f;
    public float wheelEndAngle = -90f;

    [Header("Station line")]
    public RectTransform needle;
    public float needleLeftX = -195f;
    public float needleRightX = 210f;

    [Header("Stations")]
    public float[] stationX  = {-170, -54, 69, 195};

    [Header("Tuning")]
    public float tuneSpeed = 1.5f;
    public float acceleration = 6f;

    [Header("Signal")]
    public float clearRadius = 8f;

    private bool initialized = false;
    private float tuning;
    private float needleX;
    private float velocity;

    private bool dragging = false;
    private float lastMouseAngle;
    private int enabledFrame;
    

    void OnEnable()
    {
        enabledFrame = Time.frameCount;

        if (!initialized)
        {
            if (stationX.Length != radio.stations.Length)
            {
                Debug.LogError("Radiotuner: stationX must have one entry per station");
                enabled = false;
                return;
            }

            float start = stationX[radio.CurrentStationIndex];
            tuning = Mathf.InverseLerp(needleLeftX, needleRightX, start) * stationX.Length;
            initialized = true;
        }

        ApplyVisuals();
        UpdateStationAndSignal();

    }

    void OnDisable()
    {
        dragging = false; // if panel is close mid tuning, it´s canceled
    }

    void Update()
    {

        float range = stationX.Length;

        HandleMouseDrag(range);

        if (!dragging)
        {
            // D/right arrow = +1 -- A/Left arrow = -1 -- Nothing = 0
            float input = Input.GetAxisRaw("Horizontal");

            velocity = Mathf.MoveTowards(velocity, input * tuneSpeed, acceleration * Time.deltaTime);
            tuning += velocity * Time.deltaTime;
        }
        else
        {
            velocity = 0f; //while tuning with cursor, the keyboard is ignored
        }

        if (tuning <= 0f || tuning >= range)
        {
            tuning = Mathf.Clamp(tuning, 0f, range);
            velocity = 0f;
        }

        ApplyVisuals();
        UpdateStationAndSignal();

    }

    private void HandleMouseDrag(float range)
    {
        if (cursorSystem == null) return;

        float radius = Mathf.Min(wheel.rect.width, wheel.rect.height) * 0.5f * wheel.lossyScale.x;

        Vector2 cursorPos = cursorSystem.virtualCursor.position;
        Vector2 dir = cursorPos - (Vector2)wheel.position;

        if (Input.GetMouseButtonDown(0) && Time.frameCount != enabledFrame && dir.magnitude <= radius)
        {
            dragging = true;
            lastMouseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        if (dragging && !Input.GetMouseButton(0)) dragging = false;
        if (!dragging) return;

        if (dir.magnitude < radius * 0.2f) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float delta = Mathf.DeltaAngle(lastMouseAngle, angle);
        lastMouseAngle = angle;

        float sweep = wheelStartAngle - wheelEndAngle;
        if (Mathf.Approximately(sweep, 0f)) return;

        tuning -= delta / sweep * range;
    }

    private void ApplyVisuals()
    {
        float t = tuning / stationX.Length;

        needleX = Mathf.Lerp(needleLeftX, needleRightX, t);

        //Dial´s positon 
        float angle = Mathf.Lerp(wheelStartAngle, wheelEndAngle, t);
        wheel.localRotation = Quaternion.Euler(0f, 0f, angle);

        Vector2 pos = needle.anchoredPosition;
        pos.x = needleX;
        needle.anchoredPosition = pos;
    }

    private void UpdateStationAndSignal()
    {
        int nearest = 0;
        float distance = Mathf.Abs(needleX - stationX[0]);

        for ( int i = 1; i < stationX.Length; i++)
        {
            float d = Mathf.Abs(needleX - stationX[i]);
            if ( d < distance)
            {
                distance = d;
                nearest = i;
            }
        }

        if (nearest != radio.CurrentStationIndex)
            radio.SelectStation(nearest);

        float boundary;
        if ( needleX >= stationX[nearest])
        {
            boundary = nearest < stationX.Length - 1
                ? (stationX[nearest] + stationX[nearest + 1]) / 2f
                : needleRightX;
        }
        else
        {
            boundary = nearest > 0
                ? (stationX[nearest - 1] + stationX[nearest]) / 2f
                : needleLeftX;
        }

        float halfWidth = Mathf.Max(Mathf.Abs(boundary - stationX[nearest]), clearRadius + 0.01f);

        float signal = 1f - Mathf.InverseLerp(clearRadius, halfWidth, distance);

        radio.SetSignal(signal);
    }
}
