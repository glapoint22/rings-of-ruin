public class BloodrootPickup : InteractableBase
{
    private readonly PlayerState bloodrootUpdate = new()
    {
        hasBloodroot = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(bloodrootUpdate);
        GameEvents.RaiseAddSpell(PickupType.Bloodroot);
    }
}