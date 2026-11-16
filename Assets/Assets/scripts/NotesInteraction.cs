using UnityEngine;
using UnityEngine.UI;

public class NotesInteraction : MonoBehaviour
{
    public GameObject bg;
    public GameObject[] pages;
    public GameObject clue;

    public bool notesOpened = false;
    int currentPage = 0;

    public void Awake()
    {
        bg.SetActive(false);
        clue.SetActive(false);
        HideAll();   
    }

    public void Update()
    {
        if (notesOpened)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
            {
                CloseNotes();
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ShowPage((currentPage + 1) % pages.Length);//next page or loop back to first page
            }
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
        clue.SetActive(true);
        ShowPage(0);
        notesOpened = true;
        Time.timeScale = 0; //freez world while open
    }

    public void CloseNotes()
    {
        if (!notesOpened) return;

        bg.SetActive(false);
        clue.SetActive(false);
        HideAll();
        notesOpened = false;
        Time.timeScale = 1; //unfreez world
    }

    void ShowPage(int index)
    {
        currentPage = index;
        for (int i = 0; i < pages.Length; i++) //loop through all pages
        {
            pages[i].SetActive(i == currentPage); //only show the current page
        }
    }

    void HideAll()
    {
        foreach (GameObject p in pages) //loop through all pages
        {
            p.SetActive(false);
        }
    }
}
