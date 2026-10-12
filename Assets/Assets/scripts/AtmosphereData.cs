//No using and MonoBehaviour because this is a data class, not a Unity component
public class Combo {
    public string mood;
    public string song;

    public Combo(string newMood, string newSong)//constructor to initialize the combo with mood and song
    {
        mood = newMood.Trim().ToLower(); // Trim and convert to lowercase for consistency
        song = newSong.Trim().ToLower(); 
    }
}

public enum FitLevel
{
    None,
    OneCorrect,
    Close,
    Perfect
}

public class AtmosphereResult
{
    public FitLevel level = FitLevel.None; // The level of fit between the player's input and the expected combo
    public int comboIndex = -1; // Index of the matched combo in the list, -1 if no match
    public string mood = ""; // The mood of the matched combo
    public string song = ""; // The song of the matched combo
}


