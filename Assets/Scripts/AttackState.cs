using UnityEngine;

public class AttackState : IEnemyState
{
    private readonly float rotationSpeed = 5f;


    public void Enter(EnemyStateContext context)
    {
        context.animator.SetBool("Attack", true);
    }

    
    public void Update(EnemyStateContext context)
    {
        FacePlayer(context);
    }


    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        if (IsPlayerOutOfAttackRange(context))
        {
            return new ChaseState();
        }
        if (context.player.playerState.isDead)
        {
            context.animator.SetBool("Player Dead", true);
            return new PatrolState();
        }

        return null;
    }


    public void Exit(EnemyStateContext context)
    {
        context.animator.SetBool("Attack", false);
    }


    private void FacePlayer(EnemyStateContext context)
    {
        Vector3 directionToPlayer = (context.player.transform.position - context.transform.position).normalized;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool IsPlayerOutOfAttackRange(EnemyStateContext context)
    {
        if (context.player == null) return false;

        float distance = Vector3.Distance(context.transform.position, context.player.transform.position);
        return distance > context.navMeshAgent.stoppingDistance;
    }
}