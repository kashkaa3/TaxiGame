using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics.Contracts;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused { get; private set; }

    [Header("Panels")]
    public GameObject pauseMenuUI;
    public GameObject confrimMenuUI;

    [Header("Buttons")]
    public Button backButton;
    public Button menuButton;

    [Header("Warning")]
    public Button yesButton;
    public Button noButton;

    [Header("Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    public string sceneName = "MenuScene";

    void Awake()
    {
        GameIsPaused = false;
        //Time.timeScale = 1f;
        confrimMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);

        backButton.onClick.AddListener(Resume);
        menuButton.onClick.AddListener(() => confrimMenuUI.SetActive(true));
        noButton.onClick.AddListener(() => confrimMenuUI.SetActive(false));
        yesButton.onClick.AddListener(GoToMenu);
    }

    void Update()
    {
        if (!Input.GetKeyDown(pauseKey)) return;

        if (!GameIsPaused) Pause(); //if esc is pressed and game is not paused, pause the game
        else if (confrimMenuUI.activeSelf) //if esc is pressed and confrim menu is active, close the confrim menu
        {
            confrimMenuUI.SetActive(false);
        }
        else Resume(); //if esc is pressed and game is paused, resume the game
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None; //virtual mouse can't be used to press buttons, so unlock the cursor
        Cursor.visible = true;
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = NotesInteraction.IsOpen ? 0f : 1f; //if notes are open, keep the game paused, otherwise resume the game
        GameIsPaused = false;
        confrimMenuUI.SetActive(false);
        GameSettings.Save();
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        //GameIsPaused = false;
        Cursor.lockState = CursorLockMode.None; //unlock the cursor so it can be used in the main menu
        Cursor.visible = true;
        GameSettings.Save();
        SceneManager.LoadScene(sceneName);
    }
}
