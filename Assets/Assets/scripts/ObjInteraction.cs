using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjInteraction : MonoBehaviour
{
    public float interactDistance = 15f;
    public float focusTime = 0.7f;

    public CursorSystem cursorV;

    float timer;
    bool isFocused = false; // Track if the player is currently focused on an interactable object

    public Image eyeIcon;
    public Sprite eyeClosed;
    public Sprite eyeOpened;
    public TextMeshProUGUI objName;


    //GameObject currentObject; 

    void Start ()
    {
        eyeIcon.sprite = eyeClosed; // Set the initial state of the eye icon to closed
    }
    void Update()
    {
        if (cursorV.inUIMode) // Check if the player is in UI mode, and if so, skip interaction logic
        { 
            return;
        }

        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit; // Store hit information


        if (Physics.Raycast(ray, out hit, interactDistance)) // Check if the raycast hits an object within the specified distance
        {
           // Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Interactable"))
            {
                if (!isFocused) // If not already focused, start the timer
                {
                    timer += Time.deltaTime;
                    if (timer >= focusTime)
                    {
                        isFocused = true;
                        eyeIcon.sprite = eyeOpened;
                        objName.text = hit.collider.gameObject.name; // Display the name of the interactable object

                        Debug.Log("ready to interact with " + hit.collider.gameObject);
                    }

                }
                if (isFocused && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))) // Check for interaction input
                {
                    Debug.Log("Interacted with " + hit.collider.gameObject);
                    
                    PhoneInteraction phone = hit.collider.GetComponent<PhoneInteraction>(); 
                    if (phone != null)
                    {
                        phone.Interact();
                    }

                    CigaretteInteraction cigarette = hit.collider.GetComponent<CigaretteInteraction>();
                    if (cigarette != null)
                    {
                        cigarette.Interact();
                    }

                    ResetInteraction();

                }
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
        
    }
    void ResetInteraction()
    {
        //currentObject = null;
        timer = 0f;
        isFocused = false;
        eyeIcon.sprite = eyeClosed; // Reset the eye icon to closed when not focused on an interactable object
        objName.text = ""; // Clear the object name display each frame
    }
}
