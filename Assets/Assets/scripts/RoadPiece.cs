using System.Collections.Generic;
using UnityEngine;

public class RoadPiece : MonoBehaviour
{
   public enum PieceType { Straight, Turn, Decision }

    public PieceType type;

    public Transform entry;
    public Transform exit;
    public Transform exitLeft;
    public Transform exitRight;

    public List<WaypointNode> orderedWaypoints;


    //Drawing the entry and exit
    void OnDrawGizmos()
    {
        if (entry != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(entry.position, 1f);
            Gizmos.DrawLine(entry.position, entry.position + entry.forward * 5f);
        }

        if (exit != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(exit.position, 1f);
            Gizmos.DrawLine(exit.position, exit.position + exit.forward * 5f);
        }

    }
}
