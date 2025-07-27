using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyStateContext
{
    public Transform transform;
    public Player player;
    public float health;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public List<Vector3> waypoints;
    public Vector3 currentWaypoint;
}