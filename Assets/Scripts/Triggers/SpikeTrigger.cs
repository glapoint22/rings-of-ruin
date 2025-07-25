public class SpikeTrigger : InteractableBase
{
    private Damage damage;

    private void Awake()
    {
        DamageInfo damageInfo = new()
        {
            damage = 13,
            source = DamageSource.Spikes
        };
        damage = new(damageInfo);
    }
    protected override void OnInteract()
    {
        GameEvents.RaiseDamage(damage);
    }
}