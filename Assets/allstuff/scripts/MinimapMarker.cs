using UnityEngine;
using UnityEngine.UI;

public class MinimapMarker : MonoBehaviour
{
    [Header("References")]
    public Transform car;
    public RectTransform minimapRect;
    public RectTransform marker;

    [Header("Limits of the world")]
    public Vector2 worldMin;
    public Vector2 worldMax;

    [Header("Zoom")]
    public float zoomScale = 3f;

    void Update()
    {
        float tx = Mathf.InverseLerp(worldMin.x, worldMax.x, car.position.x);
        float ty = Mathf.InverseLerp(worldMin.y, worldMax.y, car.position.z);

        minimapRect.localScale = new Vector3(zoomScale, zoomScale, 1f);

        float mapW = minimapRect.rect.width * zoomScale;
        float mapH = minimapRect.rect.height * zoomScale;

        minimapRect.anchoredPosition = new Vector2(
            -(tx - 0.5f) * mapW,
            -(ty - 0.5f) * mapH
        );

        marker.anchoredPosition = Vector2.zero;

    }
}
