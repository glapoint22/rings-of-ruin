using UnityEngine;

public class Ruinwalker : EnemyStateMachine
{
    protected override IEnemyState GetInitialState()
    {
        return new IdleState();
    }


    public void DealDamage()
    {
        DamageInfo damageInfo = new()
        {
            damage = 15,
            source = DamageSource.Ruinwalker
        };
        GameEvents.RaiseDamage(new Damage(damageInfo));
    }
}