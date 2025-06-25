public class TimeDilationPickup : InteractableBase
{
    private readonly TimeDilation timeDilation = new();
    protected override void OnInteract()
    {
        GameEvents.RaisePickup(timeDilation, PickupType.TimeDilation);
    }
}