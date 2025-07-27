using UnityEngine;
using System.Linq;

public class PatrolState : IEnemyState
{
    private const float CONTINUE_PATROL_CHANCE = 0.7f; // 70% chance to continue patrolling
    private float detectionRange = 10f;

    private float lineOfSightAngle = 90f;

    public void Enter(EnemyStateContext context)
    {
        context.animator.SetBool("Patrol", true);
        context.navMeshAgent.stoppingDistance = 0;

        // Get all waypoints except the current waypoint
        var availableWaypoints = context.waypoints.Where(wp => wp != context.currentWaypoint).ToList();


        if (availableWaypoints.Count > 0)
        {
            // Pick a random waypoint from the available ones
            int randomIndex = Random.Range(0, availableWaypoints.Count);
            context.currentWaypoint = availableWaypoints[randomIndex];

            // Set the destination to the target waypoint
            context.navMeshAgent.SetDestination(context.currentWaypoint);
        }
    }


    public void Exit(EnemyStateContext context)
    {
        // Set the patrol animation to false
        context.animator.SetBool("Patrol", false);
    }


    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        if (context.navMeshAgent.velocity.magnitude == 0 && context.navMeshAgent.remainingDistance <= context.navMeshAgent.stoppingDistance)
        {
            // Make random decision
            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= CONTINUE_PATROL_CHANCE)
            {
                return new PatrolState();
            }
            else
            {
                return new IdleState();
            }
        }

        if (IsPlayerInRange(context) && IsInLineOfSight(context) && !context.player.playerState.isDead)
        {
            return new ChaseState();
        }
        return null;

    }


    public void Update(EnemyStateContext context) { }


    private bool IsPlayerInRange(EnemyStateContext context)
    {
        if (context.player == null) return false;

        float distance = Vector3.Distance(context.transform.position, context.player.transform.position);
        return distance <= detectionRange;
    }



    private bool IsInLineOfSight(EnemyStateContext context)
    {
        // Get direction from player to target
        Vector3 directionToTarget = (context.player.transform.position - context.transform.position).normalized;

        // Get enemy's forward direction
        Vector3 enemyForward = context.transform.forward;

        // Calculate angle between player forward and direction to target
        float angle = Vector3.Angle(enemyForward, directionToTarget);

        // Check if target is within the line of sight angle (half on each side)
        return angle <= lineOfSightAngle * 0.5f;
    }
}