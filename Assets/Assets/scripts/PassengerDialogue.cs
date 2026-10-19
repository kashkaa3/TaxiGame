using System.Collections;
using UnityEngine;


public class PassengerDialogue : MonoBehaviour
{
    [Header("Grapf for passenger")]
    public DialogueGraph[] passengerGraph;

    DialogueManager dialogueManager; // Reference to the DialogueManager.cs

    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>(); // Find the DialogueManager in the scene
        StartPassengerDialogue();
    }
   
    public void StartPassengerDialogue()
    {
        int num = GameStateManager.Instance.PassengerNumber;

        if (num < 0 || num >= passengerGraph.Length) // safety check to ensure the index is within bounds
        {
            Debug.LogWarning("No dialogue for passenger " + num);
            return;
        }
        dialogueManager.StartDialogue(passengerGraph[num]); // Start the dialogue for the current passenger
    }
}
