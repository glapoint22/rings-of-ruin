public class FireballAcquire : InteractableBase
{
    private readonly PlayerState fireballAcquired = new()
    {
        hasFireball = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaiseAddSpell(SpellType.Fireball);
        GameEvents.RaisePlayerStateUpdate(fireballAcquired);
    }
}