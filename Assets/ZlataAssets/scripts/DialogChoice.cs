using UnityEngine;

public class DialogChoice: MonoBehaviour
{
    [Header ("Emotion")] // This is just for the editor, it doesn't do anything
    public string emotionName;
    public EmoteTypes emoteType;
    public int emoteValue = 1;

    //[HideInInspector]
    public DialogueManager dialogueManager;
    EmotionSystem emotionSystem;

    void Awake()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        emotionSystem = FindAnyObjectByType<EmotionSystem>();
    }
    public void MakeChoice() // This method will be called when the player clicks on this choice
    {
        emotionSystem.AddEmotion(emoteType, emoteValue); // Update the emotion system with the chosen emotion
        dialogueManager.ChooseEmotion(emotionName); // Continue the dialogue based on the choice made
    }

}
