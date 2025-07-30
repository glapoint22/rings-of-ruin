using UnityEngine;
using System.Linq;

public class PatrolState : EnemyState, IEnemyState
{
    private const float CONTINUE_PATROL_CHANCE = 0.7f; // 70% chance to continue patrolling


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


    public void Update(EnemyStateContext context) { }
    

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


    public void Exit(EnemyStateContext context)
    {
        // Set the patrol animation to false
        context.animator.SetBool("Patrol", false);
    }
}