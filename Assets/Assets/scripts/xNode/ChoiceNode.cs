using System;
using System.Collections.Generic;
using XNode;

[Serializable]
public struct ComboEntry
{
    public string song;
    public string mood;
    public int points;
}

public class ChoiceNode : BaseNode
{
    [Input(backingValue = ShowBackingValue.Never)] public EmptyStruct entry;

    [Output(dynamicPortList = true)] public List<ComboEntry> combos;
    [Output(backingValue = ShowBackingValue.Never)] public EmptyStruct wrong;

    // converts the list in the node into Combo objects for AtmosphereController
    public List<Combo> GetCombos()
    {
        List<Combo> result = new List<Combo>();
        if (combos == null) return result;

        foreach (ComboEntry entry in combos)
        {
            result.Add(new Combo(entry.mood, entry.song));
        }
        return result;
    }

    public int GetPoints(int index)
    {
        return combos[index].points;
    }

    public BaseNode GetNextForCombo(int index) { return Follow("combos " + index); }

    public BaseNode GetNextWrong () { return Follow("wrong");  }

    public override BaseNode GetNext() { return GetNextWrong(); }
        
}
