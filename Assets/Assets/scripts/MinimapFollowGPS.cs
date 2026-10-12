using UnityEngine;

public class MinimapFollowGPS : MonoBehaviour
{
    public Transform target;
    public float height = 50f;

    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y + height, target.position.z);
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
 
}
