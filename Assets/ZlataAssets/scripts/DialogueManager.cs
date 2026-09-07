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

    [Header ("Choices")]
    public GameObject angrySphere;
    public GameObject happySphere;
    public GameObject sadSphere;
    public GameObject silentSphere;


    [Header ("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Systems")]
    public EmotionSystem emotionSystem;

    [Header ("Typing")]
    public float typingSpeed = 0.1f; // Time in seconds between each character being typed


    void Awake()
    {
        dialoguePanel.SetActive(false);
        HideChoices();
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
        HideChoices();
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

        if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
        else
        {
            HideChoices();

            float waitTime = ReadingTime(line);
            yield return new WaitForSeconds(waitTime);

            ContinueDialogue();
        }
    }

    float ReadingTime(string text)
    {
        int wordCount = text.Split(' ').Length; // Count the number of words in the text
        return 1.5f + (wordCount * 0.18f); //base time + time per word
    }

    /////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////CHOICES AND EMOTION SYSTEM/////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////
    public void ChooseEmotion(string emotionName) // This method will be called to continue the dialogue based on the choice made
    {
        for (int i = 0; i < story.currentChoices.Count; i++)
        {
            if (story.currentChoices[i].text == emotionName)
            {
                story.ChooseChoiceIndex(i); // Choose the choice at the corresponding index
                ContinueDialogue();
                //StartCoroutine(ContinueAfterPause()); // Wait for a few seconds before continuing the story
                break;
            }
        }
    }

    public void ShowChoices() { 
        HideChoices();

        //float waitTime = ReadingTime(text);
        //yield return new WaitForSeconds(waitTime * 0.05f);

        RandomizeChoicePos();

        foreach (Choice choice in story.currentChoices)
        {
            if (choice.text == "Angry")
                angrySphere.SetActive(true);

            if (choice.text == "Happy")
                happySphere.SetActive(true);

            if (choice.text == "Sad")
                sadSphere.SetActive(true);

            if (choice.text == "Silent")
                silentSphere.SetActive(true);
        }

    }

    public void HideChoices() {
        angrySphere.SetActive(false);
        happySphere.SetActive(false);
        sadSphere.SetActive(false); 
        silentSphere.SetActive(false);
    }

    void RandomizeChoicePos() {
        Transform[] shuffledPoints = (Transform[]) spawnPoints.Clone(); // Clone the spawn points array to avoid modifying the original

        for (int i = 0; i < shuffledPoints.Length; i++) // Shuffle the spawn points
        {
            Transform temp = shuffledPoints[i]; // Store the current spawn point in a temporary variable
            int randomIndex = Random.Range(i, shuffledPoints.Length); // Generate a random index from the current index to the end of the array
            shuffledPoints[i] = shuffledPoints[randomIndex]; // Swap the current spawn point with the spawn point at the random index
            shuffledPoints[randomIndex] = temp; // Swap the spawn point at the random index with the current spawn point (using the temporary variable)
        }

        GameObject[] activeChoices = {
            angrySphere,
            happySphere,
            sadSphere,
            silentSphere
        };

        for (int i = 0; i < activeChoices.Length; i++) // Set the position of each choice to the position of the corresponding shuffled spawn point
        {
            activeChoices[i].transform.position = shuffledPoints[i].position;
        }
    }
}
