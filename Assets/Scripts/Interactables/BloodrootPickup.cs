public class BloodrootPickup : InteractableBase
{
    public override void Interact()
    {
        Pickup(PickupType.Bloodroot);
        base.Interact();
    }
}