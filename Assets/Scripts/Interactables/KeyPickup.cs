public class KeyPickup : InteractableBase
{
    private readonly Key key = new();
    protected override void OnInteract()
    {
        GameEvents.RaisePickup(key, PickupType.Key);
        GameEvents.RaiseKeyPickup();
    }
}