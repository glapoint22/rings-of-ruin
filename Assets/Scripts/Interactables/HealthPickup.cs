public class HealthPickup : InteractableBase
{
    public override void Interact()
    {
        Pickup(PickupType.Health);
        base.Interact();
    }
}
