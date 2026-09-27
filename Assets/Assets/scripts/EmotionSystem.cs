using UnityEngine;

public class EmotionSystem : MonoBehaviour 
{
    public int angryPoints;
    public int happyPoints;
    public int sadPoints;
    public int silentPoints;

    public void AddEmotion(EmoteTypes emoteType, int value) // This method will be called to update the emotion points
    {
        switch (emoteType)
        {
            case EmoteTypes.Angry:
                angryPoints += value;
                break;
            case EmoteTypes.Happy:
                happyPoints += value;
                break;
            case EmoteTypes.Sad:
                sadPoints += value;
                break;
            case EmoteTypes.Silent:
                silentPoints += value;
                break;
        }
        Debug.Log("Angry: " + angryPoints);
        Debug.Log("Happy: " + happyPoints);
        Debug.Log("Sad: " + sadPoints);
        Debug.Log("Silent: " + silentPoints);
    }
}

