using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("References")]
    public CarPathFollower car;
    public RoadPiece[] startPieces;

    [Header("Pieces")]
    public RoadPiece[] straightPrefabs;
    public RoadPiece[] turnPrefabs;

    [Header("Config")]
    public int lookAheadCount = 9;
    [Range(0f, 1f)] public float turnChance = 0.25f;
    public int waypointsBehindToKeep = 3;

    private Transform lastExit;

    private class SpawnedPiece
    {
        public RoadPiece piece;
        public int lastWaypointIndex;
    }
    private Queue<SpawnedPiece> spawned = new Queue<SpawnedPiece> ();


    void Awake()
    {
        if (car == null || startPieces == null || startPieces.Length == 0 || straightPrefabs.Length == 0)
        {
            Debug.LogError("MapGenerator: missing references in the inspector");
            enabled = false;
            return;
        }

        car.waypoints = new List<WaypointNode>();

        for (int i = 0; i < startPieces.Length; i++)
        {
            if (startPieces[i] == null)
            {
                Debug.LogError("MapGenerator: empty space in startPiece");
                enabled = false;
                return;
            }

            List<WaypointNode> wps = startPieces[i].orderedWaypoints;

            int from = (i == 0) ? 0 : 1;
            car.waypoints.AddRange(wps.GetRange(from, wps.Count - from));

            spawned.Enqueue(new SpawnedPiece
            {
                piece = startPieces[i],
                lastWaypointIndex = car.waypoints.Count - 1
            });

            if (!startPieces[i].gameObject.scene.IsValid())
            {
                Debug.LogError("MapGenerator:Start pieces, element " + i + "('" + startPieces[i].name + "') is a prefab from porject, it has to be a copy of the scene");
                enabled=false;
                return;
            }
        }

        lastExit = startPieces[startPieces.Length - 1].exit;

    }

  
    void Update()
    {
        int remaining = car.waypoints.Count - car.CurrentWaypointIndex;
        if (remaining <= lookAheadCount)
            GenerateNextPiece();

        DestroyOldPieces();
    }

    void GenerateNextPiece()
    {
        RoadPiece prefab = ChooseRandomPiece();
        RoadPiece instance = Instantiate(prefab);

        AlignPieceToExit(instance.transform, instance.entry, lastExit);

        List<WaypointNode> wps = instance.orderedWaypoints;
        car.waypoints.AddRange(wps.GetRange(1, wps.Count - 1));
        lastExit = instance.exit;

        spawned.Enqueue(new SpawnedPiece
        {
            piece = instance,
            lastWaypointIndex = car.waypoints.Count - 1
        });

    }


    RoadPiece ChooseRandomPiece()
    {
        bool useTurn = turnPrefabs.Length > 0 && Random.value < turnChance;
        RoadPiece[] pool = useTurn ? turnPrefabs : straightPrefabs;
        return pool[Random.Range(0, pool.Length)];
    }

    void DestroyOldPieces()
    {
        while (spawned.Count > 0 && car.CurrentWaypointIndex > spawned.Peek().lastWaypointIndex + waypointsBehindToKeep)
        {
            SpawnedPiece old = spawned.Dequeue();
            Destroy(old.piece.gameObject);
        }
    }

    void AlignPieceToExit(Transform piece, Transform entry, Transform targetExit)
    {
        Quaternion rotationFix = targetExit.rotation * Quaternion.Inverse(entry.rotation);
        piece.rotation = rotationFix * piece.rotation;

        piece.position += targetExit.position - entry.position;
    }

}
