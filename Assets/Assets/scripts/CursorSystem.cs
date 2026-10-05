using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    public GameObject mainCursor;
    public RectTransform uiArea;
    public RectTransform virtualCursor;

    public bool inUIMode = false;
    public float sensitivity = 1600f;

    public System.Action OnExitUIMode; // Event to notify when exiting UI mode

    void Awake()
    {
        InGameMode();
    }

    private void Update()
    {
        //Set the cursor to be locked and invisible
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;


        if (inUIMode)
        {
            if (Input.GetKeyDown(KeyCode.Q)|| Input.GetMouseButtonDown(1))
            {
                InGameMode();
                OnExitUIMode?.Invoke(); // Invoke the event when exiting UI mode
            }

            MoveVirtualCursor();
        }
    }

    void MoveVirtualCursor()
    {
        // Get the mouse movement delta
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 pos = virtualCursor.anchoredPosition + delta * sensitivity *Time.deltaTime; // Adjust the multiplier for sensitivity

        //Limit the virtual cursor to the bounds of the UI area
        float halfWidth = uiArea.rect.width / 2f;
        float halfHeight = uiArea.rect.height / 2f;

        pos.x = Mathf.Clamp(pos.x, -halfWidth, halfWidth);
        pos.y = Mathf.Clamp(pos.y, -halfHeight, halfHeight);

        virtualCursor.anchoredPosition = pos; // Update the virtual cursor position
    }

    public void InUIMode()
    {
        inUIMode = true;
        mainCursor.SetActive(false);
        virtualCursor.gameObject.SetActive(true);
        virtualCursor.anchoredPosition = Vector2.zero; // Start the virtual cursor in the center of the UI area
    }

    public void InGameMode()
    {
        inUIMode = false;
        mainCursor.SetActive(true);
        virtualCursor.gameObject.SetActive(false);
    }
}
