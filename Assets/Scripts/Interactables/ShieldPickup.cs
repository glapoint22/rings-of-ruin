public class ShieldPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Shield);
        base.Interact();
    }
}