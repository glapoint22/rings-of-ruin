public class TimeDilationPickup : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseTimeDilationPickup();
    }
}