using UnityEngine;
using XNode;

public class TutorialNode : BaseNode
{
    [Input] public EmptyStruct entry;
    [Output] public EmptyStruct exit;

    public string song = "";
    public string mood = "";
    public float holdTime = 4f;

    public override BaseNode GetNext()
    {
        return Follow("exit");
    }
}
