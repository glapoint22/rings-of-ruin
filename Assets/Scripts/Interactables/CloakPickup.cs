public class CloakPickup : InteractableBase
{
    public override void Interact()
    {
        Pickup(PickupType.Cloak);
        base.Interact();
    }
}