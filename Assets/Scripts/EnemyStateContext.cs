using UnityEngine;
using System.Collections.Generic;

public class EnemyStateContext
{
    public Transform transform;
    public Transform playerTransform;
    public float health;
    
    // Add pathfinding support
    public Pathfinder pathfinder;
    public PathMover pathMover;
    public List<Vector3> waypoints;
}