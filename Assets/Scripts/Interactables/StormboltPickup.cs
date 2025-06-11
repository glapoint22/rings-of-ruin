public class StormboltPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Stormbolt);
        base.Interact();
    }
}