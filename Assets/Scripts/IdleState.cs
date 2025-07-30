using UnityEngine;

public class IdleState : EnemyState, IEnemyState
{
    private float idleTimer;
    

    public void Enter(EnemyStateContext context)
    {
        idleTimer = Random.Range(10f, 20f);
    }


    public void Update(EnemyStateContext context)
    {
        idleTimer -= Time.deltaTime;
    }


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


    public void Exit(EnemyStateContext context) { }
}