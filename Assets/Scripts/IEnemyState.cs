using UnityEngine;

public interface IEnemyState
{
    void Enter(EnemyStateContext context);
    void Update(EnemyStateContext context);
    IEnemyState ShouldTransition(EnemyStateContext context);
    void Exit(EnemyStateContext context);
}