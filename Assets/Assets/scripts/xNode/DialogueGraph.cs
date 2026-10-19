using UnityEngine;
using XNode;
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueGraph")]
public class DialogueGraph: NodeGraph
{
    public StartNode FindStart()
    {
        foreach (Node node in nodes)
        {
            if (node is StartNode)
            {
                return node as StartNode;
            }
        }
        return null;
    }
    
}
