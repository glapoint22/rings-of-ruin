public class TimeDilationPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.TimeDilation);
        base.Interact();
    }
}
