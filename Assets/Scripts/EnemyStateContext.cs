using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyStateContext
{
    public Transform transform;
    public Transform player;
    public float health;

    public NavMeshAgent navMeshAgent;

    public Animator animator;
    
    // Add pathfinding support
    // public Pathfinder pathfinder;
    // public PathMover pathMover;
    public List<Vector3> waypoints;
    public Vector3 spawnPoint;
    // public Vector3 targetWaypoint;
}