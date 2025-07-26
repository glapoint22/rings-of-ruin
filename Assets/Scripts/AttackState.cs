using UnityEngine;

public class AttackState : IEnemyState
{
    private float lineOfSightAngle = 90f;

    public void Enter(EnemyStateContext context)
    {
        context.animator.SetBool("Attack", true);
    }

    public void Exit(EnemyStateContext context)
    {
        context.animator.SetBool("Attack", false);
    }

    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        if (IsPlayerOutOfAttackRange(context))
        {
            return new ChaseState();
        }
       
        return null;
    }

    public void Update(EnemyStateContext context)
    {
        // throw new System.NotImplementedException();
    }


    private bool IsPlayerOutOfAttackRange(EnemyStateContext context)
    {
        if (context.player == null) return false;

        float distance = Vector3.Distance(context.transform.position, context.player.position);
        return distance > context.navMeshAgent.stoppingDistance;
    }


    private bool IsInLineOfSight(EnemyStateContext context)
    {
        // Get direction from player to target
        Vector3 directionToTarget = (context.player.position - context.transform.position).normalized;

        // Get enemy's forward direction
        Vector3 enemyForward = context.transform.forward;

        // Calculate angle between player forward and direction to target
        float angle = Vector3.Angle(enemyForward, directionToTarget);

        // Check if target is within the line of sight angle (half on each side)
        return angle <= lineOfSightAngle * 0.5f;
    }
}