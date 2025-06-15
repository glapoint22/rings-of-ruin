public class StormboltPickup : InteractableBase
{
    public override void Interact()
    {
        Pickup(PickupType.Stormbolt);
        base.Interact();
    }
}