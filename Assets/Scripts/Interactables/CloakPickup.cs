public class CloakPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Cloak);
        base.Interact();
    }
}