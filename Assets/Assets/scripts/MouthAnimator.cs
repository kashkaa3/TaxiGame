using UnityEngine;
using UnityEngine.UI;

public class MouthAnimator : MonoBehaviour
{
    [System.Serializable]
    public class CharacterMouth
    {
        public string name;             // For inspector only
        public Sprite baseSprite;       // the sprite that DialogueNode sets
        public Sprite mouthOpenSprite;  // The open mouth layer
    }

    [Header("References")]
    public DialogueManager dialogueManger;
    public Image characterImage; // ImagePassenger1
    public Image mouthImage;     // the son "Mouth"

    [Header("Characters")]
    public CharacterMouth[] characters;

    [Header("Settings")]
    public float flapInterval = 0.12f; //seconds between open and close

    private Sprite lastBaseSprite;
    private bool hasMouth;
    private bool mouthOpen;
    private float timer;

    void Update()
    {
        //If base image is hidden, the mouth also hides
        if (!characterImage.enabled)
        {
            timer = 0f;
            mouthOpen = false;
            mouthImage.enabled = false;
            return;
        }

        // If the character changed, search it´s mouth
        if (characterImage.sprite != lastBaseSprite)
        {
            lastBaseSprite = characterImage.sprite;
            SelectMouth(lastBaseSprite);
        }

        if (dialogueManger.isTalking && hasMouth)
        {
            timer += Time.deltaTime;
            if ( timer >= flapInterval)
            {
                timer = 0f;
                mouthOpen = !mouthOpen;
                mouthImage.enabled = mouthOpen;
            }
        }
        else
        {
            timer = 0f;
            mouthOpen = false;
            mouthImage.enabled = false;
        }
    }

    void SelectMouth(Sprite baseSprite)
    {
        hasMouth = false;
        mouthImage.enabled = false;

        foreach ( CharacterMouth c in characters)
        {
            if (c.baseSprite == baseSprite)
            {
                mouthImage.sprite = c.mouthOpenSprite;
                hasMouth = true;
                return;
            }
        }
    }
}
