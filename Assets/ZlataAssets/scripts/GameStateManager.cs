using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool IsDriving = false;
    public bool RideFinished = false;

    private void Awake()
    {
        Instance = this;
    }
}
