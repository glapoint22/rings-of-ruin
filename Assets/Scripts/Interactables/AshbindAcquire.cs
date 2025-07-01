public class AshbindAcquire : InteractableBase
{
    private readonly PlayerState ashbindAcquired = new()
    {
        hasAshbind = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaiseAddSpell(SpellType.Ashbind);
        GameEvents.RaisePlayerStateUpdate(ashbindAcquired);
    }
}