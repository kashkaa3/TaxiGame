using XNode;

public class StartNode : BaseNode
{
    [Output(backingValue = ShowBackingValue.Never)] public EmptyStruct exit;

    public override BaseNode GetNext() { return Follow("exit"); }
}
