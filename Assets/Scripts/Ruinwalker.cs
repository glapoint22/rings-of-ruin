using UnityEngine;

public class Ruinwalker : EnemyStateMachine
{
    protected override IEnemyState GetInitialState()
    {
        return new IdleState();
    }
}