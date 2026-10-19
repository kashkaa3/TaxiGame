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

    BaseNode nextNode;

    [Header("TEMPORARY TEST - delete later")]
    public WaveDisplay waveDisplay;


    void Awake()
    {
        dialoguePanel.SetActive(false);
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
        waveDisplay.previewMode = true; // Test!!!
        characterName.text = ""; // Test!!!
        Debug.Log("Dialogue Ended"); 
    }

    /////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////DIALOGUE MECHANICS//////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////
    
    IEnumerator RunChoice (ChoiceNode choiceNode)
    {
        characterName.text = ""; //hide name while wave runs
        atmosphere.Checkpoint(choiceNode.GetCombos());

        yield return new WaitUntil(() => !atmosphere.isInputActive); //waits until timer is over

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

        dialogueText.text = " ";
        isTalking = true; // Set isTalking to true when starting to type the line
        foreach (char letter in line.dialogLine)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTalking = false; // Set isTalking to false after the line has been fully typed

        yield return new WaitForSeconds(ReadingTime(line.dialogLine)); // Wait for the reading time before continuing
    }

    float ReadingTime(string text)
    {
        int wordCount = text.Split(' ').Length; // Count the number of words in the text
        return 1.5f + (wordCount * 0.18f); //base time + time per word
    }
}
