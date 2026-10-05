using Ink.Runtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink")]
    Story story; // The current Ink story being played

    [Header ("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterName;

    [Header ("Typing")]
    public float typingSpeed = 0.1f; // Time in seconds between each character being typed


    void Awake()
    {
        dialoguePanel.SetActive(false);
    }
    //////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////DIALOGUE ACTTIONS ////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    public void StartDialogue(TextAsset inkJSON)
    {
        Debug.Log("Starting Dialogue");
        story = new Story(inkJSON.text);
        dialoguePanel.SetActive(true); // Show the dialogue panel
        ContinueDialogue();
    }


    public void ContinueDialogue() // This method will be called to continue the story
    {
        if (!story.canContinue)
        {
            EndDialogue(); 
            return;
        }

        string text = story.Continue(); // Get the next line of dialogue 
        string[] partsOfText = text.Split(':'); // Split the text into parts based on the colon character


        string currentName = ""; // Initialize an empty string for the character name
        string dialogueLine = text;

        if (partsOfText.Length >= 2)
        {
            currentName = partsOfText[0]; // Set the name text to the first part of the split text
            dialogueLine = partsOfText[1]; // Set the dialogue text to the second part of the split text
        }
        else
        {
            currentName = "";
            dialogueLine = text;
        }

        characterName.text = currentName; // Update the character name text in the UI
        StopAllCoroutines();
        StartCoroutine(TypeLine(dialogueLine));
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        Debug.Log("Dialogue Ended");
    }

    /////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////DIALOGUE MECHANICS//////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    IEnumerator TypeLine(string line)
    {
        dialogueText.text = " ";
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        float waitTime = ReadingTime(line);
        yield return new WaitForSeconds(waitTime);

        ContinueDialogue();
    }

    float ReadingTime(string text)
    {
        int wordCount = text.Split(' ').Length; // Count the number of words in the text
        return 1.5f + (wordCount * 0.18f); //base time + time per word
    }
}
