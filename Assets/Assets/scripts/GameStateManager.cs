using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool IsDriving = false; //false for testing 
    public bool RideFinished = true; //true for testing

    public int PassengerNumber = 0; //0 for testing

    private void Awake()
    {
        Instance = this;

        // Ensure that the GameStateManager persists across scenes
        //if (Instance != null && Instance != this)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
    }

}
