using UnityEngine;

public class IdleState : IEnemyState
{
    private float idleTimer;
    private float detectionRange = 10f;

    private float lineOfSightAngle = 90f;

    public void Enter(EnemyStateContext context)
    {
        idleTimer = Random.Range(10f, 20f);
    }

    public void Update(EnemyStateContext context)
    {
        idleTimer -= Time.deltaTime;
    }

    public void Exit(EnemyStateContext context) { }

    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        if (IsPlayerInRange(context) && IsInLineOfSight(context) && !context.player.playerState.isDead)
        {
            return new ChaseState();
        }

        if (idleTimer <= 0)
        {
            return new PatrolState();
        }

        return null; // Stay in idle
    }

    private bool IsPlayerInRange(EnemyStateContext context)
    {
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