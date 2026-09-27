using System.Collections.Generic;
using UnityEngine;


public class CarPathFollower : MonoBehaviour
{

    private WaypointNode pendingDecisionNode;
    private int pendingDecisonIndex;

    public List<WaypointNode> waypoints;

    public float moveSpeed = 1f;
    public float rotationSpeed = 2f;

    private int currentWaypoint = 0;
    private bool waitingForDecision = false;

    void Update()
    {
        //For when there are no waypoints left
        if (currentWaypoint >= waypoints.Count)
            return;

        WaypointNode target = waypoints[currentWaypoint];

        //Decission window opens

        if (target.isDecisionPoint && !waitingForDecision )
        {
            waitingForDecision = true;

            pendingDecisionNode = target;
            pendingDecisonIndex = currentWaypoint;

            Debug.Log("Choose route! A= Left or D= Right");
        }

        //Decision
        if (waitingForDecision)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ChooseLeftRoute();
            }

            if(Input.GetKeyDown(KeyCode.D))
            {
                ChooseRightRoute();
            }
        }


        //Moving to the waypoint
        Vector3 direction = (target.transform.position - transform.position).normalized;

        // Rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime);

        float alignment = Vector3.Dot(transform.forward, direction);
        float speedFactor = Mathf.Clamp01(alignment);

        //Moving forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        //Distance to waypoint
        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        if (distance <0.5f)
        {
            //If we didn´t choose on time
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
        waitingForDecision= false;
        waypoints.RemoveAt(pendingDecisonIndex);
        waypoints.InsertRange(pendingDecisonIndex, pendingDecisionNode.rightRoute);
        currentWaypoint = pendingDecisonIndex;
        Debug.Log("Default route");
    }
}




    