using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public CursorSystem cursorV;

    public float mouseSensitivity = 100f;

    public float maxLookY = 15f;
    public float maxLookX = 8f;

    float camRotationY;
    float camRotationX;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }
    void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        if (cursorV.inUIMode) { // Check if the player is in UI mode
            return;
        }

        camRotationY += Input.GetAxis("Mouse X") * mouseSensitivity * GameSettings.Sensitivity * Time.deltaTime;
        camRotationY = Mathf.Clamp(camRotationY, -maxLookX/1.5f, maxLookX);
        camRotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * GameSettings.Sensitivity * Time.deltaTime;
        camRotationX = Mathf.Clamp(camRotationX, -maxLookY/2f, maxLookY);
        transform.localRotation = Quaternion.Euler(camRotationX, camRotationY, 0f);
    }
}

