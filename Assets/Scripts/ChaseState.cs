using UnityEngine;

public class ChaseState : IEnemyState
{
    public void Enter(EnemyStateContext context)
    {
        Debug.Log("Entering Chase State");
    }

    public void Exit(EnemyStateContext context)
    {
        
    }

    public IEnemyState ShouldTransition(EnemyStateContext context)
    {
        return null; // No transition logic for now
    }

    public void Update(EnemyStateContext context)
    {
        Debug.Log("Chasing Player");
    }
}