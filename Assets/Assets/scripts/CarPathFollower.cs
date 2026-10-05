using System.Collections.Generic;
using UnityEngine;

public class CarPathFollower : MonoBehaviour
{
    private WaypointNode pendingDecisionNode;
    private int pendingDecisonIndex;

    public List<WaypointNode> waypoints;

    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float arrivalRadius = 2f;

    private int currentWaypoint = 0;
    private bool waitingForDecision = false;

    void Start()
    {
        // Encara el coche hacia el primer waypoint de golpe, sin animación de giro
        if (waypoints.Count > 0)
        {
            Vector3 initialDirection = (waypoints[0].transform.position - transform.position).normalized;
            Vector3 flatInitialDirection = new Vector3(initialDirection.x, 0f, initialDirection.z).normalized;

            if (flatInitialDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(flatInitialDirection);
            }
        }
    }

    void Update()
    {
        if (currentWaypoint >= waypoints.Count)
            return;

        WaypointNode target = waypoints[currentWaypoint];

        if (target.isDecisionPoint && !waitingForDecision)
        {
            waitingForDecision = true;
            pendingDecisionNode = target;
            pendingDecisonIndex = currentWaypoint;
            Debug.Log("Choose route! A= Left or D= Right");
        }

        if (waitingForDecision)
        {
            if (Input.GetKeyDown(KeyCode.A))
                ChooseLeftRoute();

            if (Input.GetKeyDown(KeyCode.D))
                ChooseRightRoute();
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z).normalized;

        float speedFactor = 1f;

        if (flatDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime);

            float alignment = Vector3.Dot(transform.forward, flatDirection);
            speedFactor = Mathf.Clamp01(alignment);
        }

        transform.position += transform.forward * moveSpeed * speedFactor * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < arrivalRadius)
        {
            if (waitingForDecision)
            {
                Debug.Log("You didn´t choose a route, choosing default route");
                ChooseDefaultRoute();
            }
            else
            {
                currentWaypoint++;
            }
        }
    }

    void ChooseLeftRoute()
    {
        waitingForDecision = false;
        waypoints.RemoveAt(pendingDecisonIndex);
        waypoints.InsertRange(pendingDecisonIndex, pendingDecisionNode.leftRoute);
        currentWaypoint = pendingDecisonIndex;
        Debug.Log("Left Route");
    }

    void ChooseRightRoute()
    {
        waitingForDecision = false;
        waypoints.RemoveAt(pendingDecisonIndex);
        waypoints.InsertRange(pendingDecisonIndex, pendingDecisionNode.rightRoute);
        currentWaypoint = pendingDecisonIndex;
        Debug.Log("Right Route");
    }

    void ChooseDefaultRoute()
    {
        waitingForDecision = false;
        waypoints.RemoveAt(pendingDecisonIndex);
        waypoints.InsertRange(pendingDecisonIndex, pendingDecisionNode.rightRoute);
        currentWaypoint = pendingDecisonIndex;
        Debug.Log("Default route");
    }
}