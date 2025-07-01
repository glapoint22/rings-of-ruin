public class BloodrootAcquire : InteractableBase
{
    private readonly PlayerState bloodrootAcquired = new()
    {
        hasBloodroot = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaiseAddSpell(SpellType.Bloodroot);
        GameEvents.RaisePlayerStateUpdate(bloodrootAcquired);
    }
}