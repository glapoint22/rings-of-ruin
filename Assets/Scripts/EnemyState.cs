using UnityEngine;

public class EnemyState
{
    private float lineOfSightAngle = 90f;
    private float detectionRange = 10f;


    protected bool IsInLineOfSight(EnemyStateContext context)
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


    protected bool IsPlayerInRange(EnemyStateContext context)
    {
        float distance = Vector3.Distance(context.transform.position, context.player.transform.position);
        return distance <= detectionRange;
    }
}