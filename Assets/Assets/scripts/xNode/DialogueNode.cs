using UnityEngine;
using XNode;
using static XNode.Node;

public class DialogueNode : BaseNode 
{
    [Input(backingValue = ShowBackingValue.Never)] public EmptyStruct entry;
    [Output(backingValue = ShowBackingValue.Never)] public EmptyStruct exit;

    public string passengerName;
    [TextArea] public string dialogLine;

    [Tooltip("Filled automatically by Tools > Localization > Sync Dialogue Graphs")]
    public string lineKey; //ID for the line in the localization system
    public Sprite sprite;

    public bool hideSprite = false;
    public bool hideName = false;

    public override BaseNode GetNext() { return Follow ("exit"); }
}
