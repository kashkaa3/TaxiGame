using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header ("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterName;
    public Image characterImage;

    [Header ("Typing")]
    public float typingSpeed = 0.1f; // Time in seconds between each character being typed

    [Header("State")]
    public bool isTalking = false; // makes character open mouth while text is being typed

    [Header("Atmosphere")]
    public AtmosphereController atmosphere;
    public AtmosphereBridge bridge;

    [Header("Skip")]
    public bool skipEnabled = true; // Flag to enable or disable skipping the dialogue
    bool skipPressed = false; // Flag to check if the skip button has been pressed

    BaseNode nextNode;

    void Awake()
    {
        dialoguePanel.SetActive(false);
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

    //////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////DIALOGUE ACTTIONS ////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    public void StartDialogue(DialogueGraph graph)
    {
        Debug.Log("Starting Dialogue");
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
        //dialoguePanel.SetActive(false); // Hide for test unhide later
        //waveDisplay.previewMode = true; // Test!!!
        //characterName.text = ""; // Test!!!
        Debug.Log("Dialogue Ended"); 
    }

    /////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////DIALOGUE MECHANICS//////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    
    IEnumerator RunChoice (ChoiceNode choiceNode)
    {
        characterName.text = ""; //hide name while wave runs
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
        characterName.text = line.passengerName; // Update the character name text in the UI

        if (characterImage != null && line.sprite != null) //change picture only if node has one
        {
            characterImage.sprite = line.sprite; // Update the character image in the UI
        }

        skipPressed = false; // Reset the skip button state at the start of typing a new line
        dialogueText.text = " ";
        isTalking = true; // Set isTalking to true when starting to type the line
        foreach (char letter in line.dialogLine)
        {
            if (skipPressed)
            {
                dialogueText.text = line.dialogLine;
                skipPressed = false;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTalking = false; // Set isTalking to false after the line has been fully typed
        
        float waitTime = ReadingTime(line.dialogLine); // Calculate the time to wait based on the length of the dialogue line
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
        int wordCount = text.Split(' ').Length; // Count the number of words in the text
        return 1.5f + (wordCount * 0.18f); //base time + time per word
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
