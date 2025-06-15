public class TimeDilationPickup : InteractableBase
{
    public override void Interact()
    {
        Pickup(PickupType.TimeDilation);
        base.Interact();
    }
}
