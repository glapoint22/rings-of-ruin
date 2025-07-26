using UnityEngine;

public class ChaseState : IEnemyState
{
    private float updateTimer;
    private float updateInterval = 0.2f;

    public void Enter(EnemyStateContext context)
    {
        context.animator.SetBool("Chase", true);
        context.navMeshAgent.SetDestination(context.player.position);
        context.navMeshAgent.stoppingDistance = 1.3f;

    }

    public void Exit(EnemyStateContext context)
    {
        context.animator.SetBool("Chase", false);
    }

    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        if (context.navMeshAgent.velocity.magnitude == 0 && context.navMeshAgent.remainingDistance <= context.navMeshAgent.stoppingDistance)
        {
            return new AttackState();
        }
        return null;
    }

    public void Update(EnemyStateContext context)
    {
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            context.navMeshAgent.SetDestination(context.player.position);
        }
    }
}