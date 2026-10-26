using UnityEngine;

public class NotesInteraction : MonoBehaviour
{
    public GameObject bg;
    public GameObject notesImage;

    public bool notesOpened = false;

    public void Awake()
    {
        bg.SetActive(false);
        notesImage.SetActive(false);    
    }

    public void Update()
    {
        if (notesOpened && (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1)))
        {
            CloseNotes();
        }
    }

    public void Interact()
    {
        if (!notesOpened)
        {
            OpenNotes();
        }
    }

    public void OpenNotes()
    {
        bg.SetActive(true);
        notesImage.SetActive(true);
        notesOpened = true;
        Time.timeScale = 0; //freez world while open
    }

    public void CloseNotes()
    {
        if (!notesOpened) return;

        bg.SetActive(false);
        notesImage.SetActive(false);
        notesOpened = false;
        Time.timeScale = 1; //unfreez world
    }
}
