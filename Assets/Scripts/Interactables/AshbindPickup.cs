public class AshbindPickup : InteractableBase
{
    private readonly Ashbind ashbind = new();

    protected override void OnInteract()
    {
        GameEvents.RaisePickup(ashbind, PickupType.Ashbind);
    }
}