using UnityEngine;
using UnityEngine.SceneManagement;

public class StartQuit : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None; //unlock the cursor so it can be used in the main menu
        Cursor.visible = true;
    }
    public void Play()
    {
        // Reset game settings to default values
        GameSettings.MusicLevel = 100;
        GameSettings.SoundLevel = 100;
        GameSettings.SensLevel = 50;
        GameSettings.Save();

        SceneManager.LoadScene("MainScene");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SavingSystem savingSystem = FindFirstObjectByType<SavingSystem>();
        if (savingSystem != null)
        {
            savingSystem.LoadGame();
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.LogError("SavingSystem not found in the scene.");
        }
    }
}


