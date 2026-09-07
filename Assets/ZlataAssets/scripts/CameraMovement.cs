using UnityEngine;

public class CameraMovement : MonoBehaviour
{
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
        camRotationY += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        camRotationY = Mathf.Clamp(camRotationY, -maxLookX/1.5f, maxLookX);
        camRotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        camRotationX = Mathf.Clamp(camRotationX, -maxLookY/2f, maxLookY);
        transform.localRotation = Quaternion.Euler(camRotationX, camRotationY+180, 0f);
    }
}

