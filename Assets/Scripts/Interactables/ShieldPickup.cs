public class ShieldPickup : InteractableBase
{
    public override void Interact()
    {
        Pickup(PickupType.Shield);
        base.Interact();
    }
}