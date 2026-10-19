using UnityEngine;
using UnityEngine.UI;

public class VirtualCursorClicker : MonoBehaviour
{
    public CursorSystem cursorSystem;

    private bool wasInUIModeLastFrame = false;

    void Update()
    {
        bool canClick = cursorSystem.inUIMode && wasInUIModeLastFrame;
        wasInUIModeLastFrame = cursorSystem.inUIMode;

        if (canClick && Input.GetMouseButtonDown(0)) 
        TryClick();
    }

    private void TryClick()
    {
        Button[] buttons = cursorSystem.uiArea.GetComponentsInChildren<Button>(false);

        foreach (Button button in buttons)
        {
            if (!button.interactable) continue;

            RectTransform rt = (RectTransform)button.transform;

            Vector3 localPoint = rt.InverseTransformPoint(cursorSystem.virtualCursor.position);

            if (rt.rect.Contains(localPoint))
            {
                Debug.Log("Virtual click on " + button.name);
                button.onClick.Invoke();
                return;
            }
        }
    }
}
