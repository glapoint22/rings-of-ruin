public class HealthPickup : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseHealthPickup();
    }
}