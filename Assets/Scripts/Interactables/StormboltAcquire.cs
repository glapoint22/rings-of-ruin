public class StormboltAcquire : InteractableBase
{
    private readonly PlayerState stormboltAcquired = new()
    {
        hasStormbolt = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaiseAddSpell(SpellType.Stormbolt);
        GameEvents.RaisePlayerStateUpdate(stormboltAcquired);
    }
}