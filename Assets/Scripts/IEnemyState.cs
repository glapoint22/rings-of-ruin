using UnityEngine;

public interface IEnemyState
{
    void Enter(EnemyStateContext context);
    void Update(EnemyStateContext context);
    void Exit(EnemyStateContext context);
    IEnemyState ShouldTransition(EnemyStateContext context);
}