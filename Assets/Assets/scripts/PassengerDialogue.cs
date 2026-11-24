using System.Collections;
using UnityEngine;


public class PassengerDialogue : MonoBehaviour
{
    [Header("Grapf for passenger")]
    public DialogueGraph[] passengerGraph;

    public float dialogueDelay = 2f; // Delay before starting the dialogue

    DialogueManager dialogueManager; // Reference to the DialogueManager.cs

    void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>(); // Find the DialogueManager in the scene
        StartCoroutine(StartAfterDelay());
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitUntil(() => !LoadingOverlay.IsLoading); // Wait until the loading overlay is no longer active
        yield return new WaitForSeconds(dialogueDelay); // Wait for the specified delay
        StartPassengerDialogue(); // Start the dialogue after the delay
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
