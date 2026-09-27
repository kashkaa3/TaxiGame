using UnityEngine;

public class MinimapFollowGPS : MonoBehaviour
{
    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;

    private Transform gpsScreen;

    void Start()
    {
        Transform[] allObjects = FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (Transform obj in allObjects) 
        {
            if (obj.name == "pasted__polySurface22")
            {
                gpsScreen = obj;
                break;
            }
        }

        if (gpsScreen == null )
        {
            Debug.LogError("Object wasn´t foudn");
            return;
        }

       
    }

    void LateUpdate()
    {

        if (gpsScreen == null)
        {
            return;
        }

        //puts the canavas ON the gps
        transform.position = gpsScreen.position + gpsScreen.TransformDirection(positionOffset);

        //Cavas has the same orientation as gps
        transform.rotation = gpsScreen.rotation * Quaternion.Euler(rotationOffset);
            
    }
 
}
