public class AshbindPickup : InteractableBase
{
    private readonly PlayerState ashbindUpdate = new()
    {
        hasAshbind = true
    };

    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(ashbindUpdate);
        GameEvents.RaiseAddSpell(PickupType.Ashbind);
    }
}