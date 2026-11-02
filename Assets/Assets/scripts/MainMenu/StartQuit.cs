using UnityEngine;
using UnityEngine.SceneManagement;

public class StartQuit : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SavingSystem savingSystem = FindObjectOfType<SavingSystem>();
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


