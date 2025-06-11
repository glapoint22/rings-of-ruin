public class BloodrootPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Bloodroot);
        base.Interact();
    }
}