using System.Diagnostics;

public class Ruinwalker : EnemyAI
{
    private DamageInfo damageInfo = new(15, DamageSource.Ruinwalker);


    protected override IEnemyState GetInitialState()
    {
        return new IdleState();
    }


    public void DealDamage()
    {
        GameEvents.RaiseDamage(new Damage(damageInfo));
    }
}