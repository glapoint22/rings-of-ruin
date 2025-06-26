public class BloodrootPickup : InteractableBase
{
    private readonly Bloodroot bloodroot = new();

    protected override void OnInteract()
    {
        GameEvents.RaisePickup(bloodroot, PickupType.Bloodroot);
    }
}