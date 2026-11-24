using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class KeywordNote : MonoBehaviour
{
    public GameObject notePanel; // The panel that contains the note UI
    public TextMeshProUGUI noteText; // The TextMeshProUGUI component that displays the note text

    void Awake()
    {
        notePanel.SetActive(false); // Ensure the note panel is hidden at the start
    }

    public void ShowNote(List<string> keywords)
    {
        string result = "";
        foreach (string k in keywords)
            result += k + "\n"; //one keyword per line 
        noteText.text = result;
        notePanel.SetActive(true); 
    }

    public void Hide()
    {
        notePanel.SetActive(false);
    }
}
