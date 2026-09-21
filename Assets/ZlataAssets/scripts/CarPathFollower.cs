using System.Collections.Generic;
using UnityEngine;


public class CarPathFollower : MonoBehaviour
{

    public float decisionDistance = 5f;

    public float decisionTime = 3f;

    private float decisionTimer;
    private WaypointNode pendingDecisionNode;
    private int pendingDecisonIndex;

    public List<WaypointNode> waypoints;

    public float moveSpeed = 1f;
    public float rotationSpeed = 5f;

    private int currentWaypoint = 0;

    private bool waitingForDecision = false;

    void Update()
    {
        //Decision
        if (waitingForDecision)
        {
            decisionTimer -= Time.deltaTime;

            if (decisionTimer <= 0f)
            {
                Debug.Log("Time out, shortest route");

                ChooseDefaultRoute();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChooseLeftRoute();
            }

            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChooseRightRoute();
            }
        }

        //For when there are not waypoints left
        if (currentWaypoint >= waypoints.Count)
            return;

        WaypointNode target = waypoints[currentWaypoint];

        //Moving to the waypoint
        Vector3 direction = (target.transform.position - transform.position).normalized;

        // Rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime);

        //Moving forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        //Distance to waypoint
        float distance = Vector3.Distance(transform.position, target.transform.position);

        //Go to next waypoint

        if (target.isDecisionPoint && !waitingForDecision && distance < decisionDistance)
        {
            waitingForDecision = true;

            decisionTimer = decisionTime;

            pendingDecisionNode = target;

            pendingDecisonIndex = currentWaypoint;

            Debug.Log("Choose route! Left arrow= Left or Right arrow= Right");
        }
        if (distance <0.5f)
        {
            currentWaypoint++;
        }
    }

    void ChooseLeftRoute()
    {
        waitingForDecision = false;

        //WaypointNode currentNode = waypoints[currentWaypoint];

        waypoints.RemoveAt(pendingDecisonIndex);

        waypoints.InsertRange(pendingDecisonIndex, pendingDecisionNode.leftRoute);

        currentWaypoint = pendingDecisonIndex;

        Debug.Log("Left Route");
    }

    void ChooseRightRoute()
    {
        waitingForDecision = false;

        //WaypointNode currentNode = waypoints[currentWaypoint];

        waypoints.RemoveAt(pendingDecisonIndex);

        waypoints.InsertRange(pendingDecisonIndex, pendingDecisionNode.rightRoute);

        currentWaypoint = pendingDecisonIndex;

        Debug.Log("Right Route");
    }

    void ChooseDefaultRoute()
    {
        waitingForDecision= false;

        //WaypointNode currentNode = pendingDecisionNode;

        waypoints.RemoveAt(pendingDecisonIndex);

        waypoints.InsertRange(pendingDecisonIndex, pendingDecisionNode.rightRoute);

        currentWaypoint = pendingDecisonIndex;

        Debug.Log("Default route");
    }
}




    