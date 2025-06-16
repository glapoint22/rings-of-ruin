public class HealthPickup : PickupInteractable
{
    protected override PickupType PickupType => PickupType.Health;

    protected override void OnInteract()
    {
        Remove();
    }
}