using UnityEngine;
using System.Linq;

public class PatrolState : IEnemyState
{
    private bool pathCompleted = false;
    private const float CONTINUE_PATROL_CHANCE = 0.7f; // 70% chance to continue patrolling

    public void Enter(EnemyStateContext context)
    {
        context.animator.SetBool("Patrol", true);

        // Get all waypoints except the spawn position
        var availableWaypoints = context.waypoints.Where(wp => wp != context.spawnPoint).ToList();


        if (availableWaypoints.Count > 0)
        {
            // Pick a random waypoint from the available ones
            int randomIndex = Random.Range(0, availableWaypoints.Count);
            context.spawnPoint = availableWaypoints[randomIndex];

            // Set the destination to the target waypoint
            context.navMeshAgent.SetDestination(context.spawnPoint);
        }
    }


    public void Exit(EnemyStateContext context)
    {
        // Set the patrol animation to false
        context.animator.SetBool("Patrol", false);
    }


    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        if (!pathCompleted) return null;

        pathCompleted = false;

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


    public void Update(EnemyStateContext context)
    {
        if (!context.navMeshAgent.pathPending && !context.navMeshAgent.hasPath && context.navMeshAgent.velocity.magnitude == 0)
        {
            pathCompleted = true;
        }
    }
}