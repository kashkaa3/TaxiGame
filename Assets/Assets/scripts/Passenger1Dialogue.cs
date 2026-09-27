using System.Collections;
using UnityEngine;


public class Passenger1Dialogue : MonoBehaviour
{
    [Header("Ink")]
    public TextAsset passenger1InkJSON; // Reference to the Ink JSON file for Passenger 1

    DialogueManager dialogueManager; // Reference to the DialogueManager.cs

    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>(); // Find the DialogueManager in the scene
        StartCoroutine(DialogueStructure()); // Start the dialogue structure
    }
   
    IEnumerator DialogueStructure()
    {
        dialogueManager.StartDialogue(passenger1InkJSON); // Start the dialogue using the Ink JSON file
        yield return new WaitForSeconds(1f); 
    }
}
