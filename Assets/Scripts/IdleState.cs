using UnityEngine;

public class IdleState : IEnemyState
{
    private float idleTimer;
    private float detectionRange = 10f;

    public void Enter(EnemyStateContext context)
    {
        idleTimer = Random.Range(2f, 5f);
        Debug.Log($"Enemy entered idle state");
    }

    public void Update(EnemyStateContext context)
    {
        idleTimer -= Time.deltaTime;
        
        if (IsPlayerInRange(context))
        {
            Debug.Log("Player detected!");
        }
    }

    public void Exit(EnemyStateContext context)
    {
        Debug.Log($"Enemy exited idle state");
    }

    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        if (IsPlayerInRange(context))
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
        if (context.playerTransform == null) return false;
        
        float distance = Vector3.Distance(context.transform.position, context.playerTransform.position);
        return distance <= detectionRange;
    }
}