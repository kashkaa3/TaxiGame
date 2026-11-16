using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool IsDriving = false; //false for testing 
    public bool RideFinished = true; //true for testing

    public int PassengerNumber = 0; //0 for testing

    public int Points = 0;

    public int TotalPassengers = 1; //Remember to change this as we add more

    public bool IsLastPassenger => PassengerNumber + 1 >= TotalPassengers;

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
