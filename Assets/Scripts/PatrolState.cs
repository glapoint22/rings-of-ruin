using UnityEngine;
using System.Linq;

public class PatrolState : IEnemyState
{
    private bool pathCompleted = false;
    private const float CONTINUE_PATROL_CHANCE = 0.7f; // 70% chance to continue patrolling

    public void Enter(EnemyStateContext context)
    {
        // Subscribe to path completion event
        GameEvents.OnPathCompleted += OnPathCompleted;

        // Get all waypoints except the spawn position
        var availableWaypoints = context.waypoints.Where(wp => wp != context.pathMover.CurrentPosition).ToList();

        if (availableWaypoints.Count > 0)
        {
            // Pick a random waypoint from the available ones
            int randomIndex = Random.Range(0, availableWaypoints.Count);
            Vector3 targetWaypoint = availableWaypoints[randomIndex];

            var path = context.pathfinder.GetPath(context.pathMover.CurrentPosition, targetWaypoint);
            context.pathMover.MoveAlongPath(path);
        }
    }

    public void Exit(EnemyStateContext context)
    {
        // Unsubscribe from path completion event
        GameEvents.OnPathCompleted -= OnPathCompleted;
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
        // Movement logic will be implemented later
    }

    private void OnPathCompleted()
    {
        pathCompleted = true;
    }
}