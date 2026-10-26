using UnityEngine;
using System.Collections.Generic;

public class AtmosphereController : MonoBehaviour
{
    public float timeLimit = 15f; // Time limit for the player to input the combo
    public bool isInputActive = false; // Flag to check if input is active
    public float timeLeft;

    public AtmosphereResult lastResult = new AtmosphereResult(); // Store the last result of the combo check

    List<Combo> combos = new List<Combo>();
    string mood = "";
    string song = "";

    public void Checkpoint(List<Combo> validCombos)
    { // Method to set the valid combos and reset the input state
        combos = validCombos;
        foreach (Combo c in validCombos)
            Debug.Log("Combo expected -> mood: '" + c.mood + "' song: '" + c.song + "'");
        mood = "";
        song = "";
        timeLeft = timeLimit;
        isInputActive = true;
    }

    public void SetMood(string id) // Method to set the mood input by the player
    {
        if (!isInputActive) return;
        mood = id.Trim().ToLower(); 
    }

    public void SetSong(string id) // Method to set the song input by the player
    {
        if (!isInputActive) return;
        song = id.Trim().ToLower(); 
    }

    public AtmosphereResult GetCurrentResult()
    {
        AtmosphereResult result = new AtmosphereResult();
        result.mood = mood;
        result.song = song;

        bool moodFound = false;
        bool songFound = false;

        for (int i = 0; i < combos.Count; i++)
        {
            bool moodMatch = (mood != "" && combos[i].mood == mood);// Check if the mood matches any of the valid combos
            bool songMatch = (song != "" && combos[i].song == song);//Check if the song matches any of the valid combos

            if (moodMatch && songMatch)
            {
                result.level = FitLevel.Perfect;
                result.comboIndex = i;
                result.moodCorrect = true;
                result.songCorrect = true;
                return result; // Return immediately if a perfect match is found
            }
            if (moodMatch) moodFound = true;
            if (songMatch) songFound = true;
        }

        result.moodCorrect = moodFound; // Set the moodCorrect flag based on whether a matching mood was found
        result.songCorrect = songFound; // Set the songCorrect flag based on whether a matching song was found

        if (moodFound && songFound) result.level = FitLevel.Close; // Both mood and song are found but not in the same combo
        else if (moodFound || songFound) result.level = FitLevel.OneCorrect; // Only one of mood or song is found
        else result.level = FitLevel.None; // Neither mood nor song is found
        return result;
    }

    void Update()
    {
        if (!isInputActive) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            isInputActive = false;
            lastResult = GetCurrentResult();
            Debug.Log("Finished: " + lastResult.level + ", combo " + lastResult.comboIndex +
                " | moodCorrect: " + lastResult.moodCorrect + ", songCorrect: " + lastResult.songCorrect);
        }

    }
}
