using UnityEngine;
using System.IO;

public class SavingSystem : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath + "/savedData.json");
    }

    public void SaveGame()
    {
        SavingData data = new SavingData(); //SavingData converts to JSON and back to object so we don't save GameStateManager directly to avoid issues with serialization

        //All data that needs to be saved should be added here
        data.passengerNumber = GameStateManager.Instance.PassengerNumber;


        string json = JsonUtility.ToJson(data);// Serialize the data to JSON
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved to: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SavingData data = JsonUtility.FromJson<SavingData>(json);// Deserialize the JSON back to the data object
            
            // Restore the saved data to the game state
            GameStateManager.Instance.PassengerNumber = data.passengerNumber;
            
            
            Debug.Log("Game loaded from: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No save file found at: " + saveFilePath);
        }
    }
}
