public class StormboltPickup : InteractableBase
{
    private readonly Stormbolt stormbolt = new();

    protected override void OnInteract()
    {
        GameEvents.RaisePickup(stormbolt, PickupType.Stormbolt);
    }
}