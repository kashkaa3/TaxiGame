using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour
{
    [Header ("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterName;
    public Image characterImage;

    [Header ("Typing")]
    public float typingSpeed = 0.1f; // Time in seconds between each character being typed
    public float koreanTypingSpeed = 1.8f; // Time in seconds between each Korean character being typed

    [Header("State")]
    public bool isTalking = false; // makes character open mouth while text is being typed

    [Header("Atmosphere")]
    public AtmosphereController atmosphere;
    public AtmosphereBridge bridge;

    [Header("Skip")]
    public bool skipEnabled = true; // Flag to enable or disable skipping the dialogue
    bool skipPressed = false; // Flag to check if the skip button has been pressed

    [Header("Keywords")]
    public KeywordNote keywordNote;
    public Color keywordColor = new Color(1f, 0.82f, 0.25f); //color of marked words

    List<string> keywords = new List<string>();

    BaseNode nextNode;
    string lastPassenger= "";

    public EndScreen endScreen;

    void Awake()
    {
        dialoguePanel.SetActive(false);
        ClearUI();
    }

    void Update()
    {
        if (!Skip()) return; // Check if the skip button is pressed and handle skipping the dialogue
        if (atmosphere.isInputActive) return;
        if (Input.GetKeyDown(KeyCode.Space)) skipPressed = true; // Allow skipping with the space bar for testing
    }

    bool Skip()
    {
        bool gameFinished = PlayerPrefs.GetInt("gameFinished", 0) == 1;
        return skipEnabled || gameFinished;
    }

    void ClearUI()
    {
        dialogueText.text = "";
        characterName.text = "";
        if (characterImage != null) characterImage.enabled = false; // Hide the character image when clearing the UI
    }

    void AddKeyword(string kword)
    {
        kword = kword.Trim();
        foreach (string k in keywords)
            if (k.ToLower() == kword.ToLower()) return;
        keywords.Add(kword);
    }

    //////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////DIALOGUE ACTTIONS ////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    public void StartDialogue(DialogueGraph graph)
    {
        Debug.Log("Starting Dialogue");
        lastPassenger = "";
        dialoguePanel.SetActive(true); // Show the dialogue panel
        StopAllCoroutines(); //stops old dialogue if one is already running
        StartCoroutine(PlayGraph(graph));
    }

    IEnumerator PlayGraph(DialogueGraph graph)
    {
        StartNode start = graph.FindStart();
        if (start == null)
        {
            Debug.LogError("No Start Node found in the Dialogue Graph.");
            yield break;
        }

        BaseNode node = start.GetNext();

        while (node != null)
        {
            DialogueNode dialogue = node as DialogueNode;
            ChoiceNode choiceNode = node as ChoiceNode;
            TutorialNode tutorial = node as TutorialNode;

            if (dialogue != null) 
            {
                yield return StartCoroutine(TypeLine(dialogue));
                node = node.GetNext();
            }
            else  if (choiceNode != null) 
            {
                yield return StartCoroutine(RunChoice(choiceNode));
                node = nextNode;
            }
            else if (tutorial != null)
            {
                yield return StartCoroutine(RunTutorial(tutorial));
                node = tutorial.GetNext();
            }
            else
            {
                node = node.GetNext();
            }
        }
        EndDialogue();
    }
    public void EndDialogue()
    {
        isTalking = false;
        Debug.Log("Dialogue Ended");

        

        if (endScreen != null && GameStateManager.Instance.IsLastPassenger)
        {
            Debug.Log("Llamando a ShowEnd");
            endScreen.ShowEnd();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////DIALOGUE MECHANICS//////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    
    IEnumerator RunChoice (ChoiceNode choiceNode)
    {
        characterName.text = ""; //hide name while wave runs
        if (keywordNote != null) keywordNote.ShowNote(keywords);
        atmosphere.noTimeLimit = choiceNode.noTimeLimit;
        atmosphere.Checkpoint(choiceNode.GetCombos());

        if (choiceNode.noTimeLimit)
        {
            float held = 0f; // how long the right combo has been held
            int lastCombo = -1; // which combo was right last frame, -1 if none

            while (true) //waits until timer is over or right combo has been held long enough
            {
                AtmosphereResult now = atmosphere.GetCurrentResult(); //get the current result of the combo check
                bool right = (now.level == FitLevel.Perfect); //check if the current result is a perfect match

                if (right && now.comboIndex == lastCombo) held += Time.deltaTime; //if the right combo is being held, increase the held time
                else held = 0f;

                lastCombo = right ? now.comboIndex : -1; //update the last combo index if right, otherwise reset to -1

                if (right && held >= choiceNode.holdTime) break;   //if the right combo has been held long enough, exit the loop
                yield return null;
            }
            atmosphere.Finish(); 
        }
        else
        {
            yield return new WaitUntil(() => !atmosphere.isInputActive); //waits until timer is over
        }

        if (keywordNote != null) keywordNote.Hide();
        keywords.Clear(); // Clear the keywords list after the choice is made

        AtmosphereResult result = atmosphere.lastResult;

        if(result.comboIndex >= 0)
        {
            GameStateManager.Instance.Points += choiceNode.GetPoints(result.comboIndex);
            nextNode = choiceNode.GetNextForCombo(result.comboIndex);
        }
        else { nextNode = choiceNode.GetNextWrong();  }
    }

    IEnumerator TypeLine(DialogueNode line)
    {
        string passengerID = string.IsNullOrEmpty(line.passengerName) ? lastPassenger : line.passengerName; // Default to "Narrator" if passenger name is empty

        string text = line.dialogLine; // Default to the original dialogue line
        string passenger = passengerID; // Default to the original passenger name

        if (!string.IsNullOrEmpty(line.lineKey)) // Check if a localization key is provided
        {
            var lineOp = DialogueLoc.GetD(line.lineKey); 
            yield return lineOp; 
            text = lineOp.Result; // Update the text with the localized dialogue line

            if (!line.hideName && !string.IsNullOrEmpty(passengerID)) // Check if a localization key for the passenger name is provided
            {
                var nameOp = DialogueLoc.GetD("Name_" + passengerID); // Get the localized passenger name using a specific key format
                yield return nameOp;
                passenger = nameOp.Result;
            }
        }

        bool speakerChanged = !line.hideName && passengerID != lastPassenger;
        if (!line.hideName) lastPassenger = passengerID; // Update the last passenger name only if it's not hidden

        characterName.text = line.hideName ? "" : passenger;
        if (characterImage != null)
        {
            characterImage.enabled = !line.hideSprite; // Show or hide the character image based on the hideSprite property
            
            if (!line.hideSprite && line.sprite != null) //change picture only if not hidden and sprite is not null
            {
                characterImage.sprite = line.sprite; // Update the character image in the UI
            }
        }

        if (speakerChanged && PassengerUI.Instance != null)
        {
            PassengerUI.Instance.PlayEnter();
        }

        string hex = ColorUtility.ToHtmlStringRGB(keywordColor);
        foreach (Match m in Regex.Matches(text, @"///(.+?)///"))
            AddKeyword(m.Groups[1].Value); // Add the keyword to the list
        bool inKeyword = false; // Flag to track if we are currently inside a keyword

        skipPressed = false; // Reset the skip button state at the start of typing a new line
        dialogueText.text = " ";
        isTalking = true; // Set isTalking to true when starting to type the line
        for (int i = 0; i < text.Length; i++)
        {
            if (skipPressed)
            {
                dialogueText.text = Regex.Replace(text, @"///(.+?)///", "<color=#" + hex + ">$1</color>");
                skipPressed = false; // Reset the skip button state after processing the text
                break;
            }

            if (i + 3 <= text.Length && text.Substring(i, 3) == "///")
            {
                dialogueText.text += inKeyword ? "</color>" : "<color=#" + hex + ">"; // Toggle the color tag for keywords
                inKeyword = !inKeyword; // Toggle the inKeyword flag when encountering the keyword delimiter
                i += 2; // Skip the next two characters as they are part of the delimiter
                continue;
            }

            char letter = text[i];
            dialogueText.text += letter; // Append the current character to the dialogue text
            yield return new WaitForSeconds(letter >= 0xAC00 ? typingSpeed * koreanTypingSpeed : typingSpeed);
        }
        isTalking = false; // Set isTalking to false after the line has been fully typed
        
        float waitTime = ReadingTime(text.Replace("///", "")); // Calculate the time to wait based on the length of the dialogue line
        float timer = 0f;
        while (timer < waitTime && !skipPressed)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        skipPressed = false; // Reset the skip button state after waiting for the reading time
    }

    float ReadingTime(string text)
    {
        float time = 1.5f;
        foreach (char c in text)
        {
            time += c >= 0xAC00 ? 0.07f : 0.035f; // Adjust reading time based on character type (Korean characters take longer)
        }
        return time;
    }

    IEnumerator RunTutorial (TutorialNode t)
    {
        string sSong = t.song.Trim().ToLower();
        string mMood = t.mood.Trim().ToLower();

        float held = 0f;

        while (true)
        {
            bool right = (sSong == "" || bridge.GetCurrentSongId().ToLower() == sSong) && 
                (mMood == "" || bridge.GetMoodId().ToLower() == mMood);
            if (right) held += Time.deltaTime;
            else held = 0f;

            if (right && held >= t.holdTime) break;

            yield return null;
        }
    }
}
