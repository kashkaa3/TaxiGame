using UnityEngine;
using XNode;
using static XNode.Node;

public class DialogueNode : BaseNode 
{
    [Input(backingValue = ShowBackingValue.Never)] public EmptyStruct entry;
    [Output(backingValue = ShowBackingValue.Never)] public EmptyStruct exit;

    public string passengerName;
    [TextArea] public string dialogLine;
    public Sprite sprite;

    public override BaseNode GetNext() { return Follow ("exit"); }
}
