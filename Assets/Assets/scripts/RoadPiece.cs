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
}
