public class KeyPickup : InteractableBase
{
    public override void Interact()
    {
        Pickup(PickupType.Key);
        base.Interact();
    }
}