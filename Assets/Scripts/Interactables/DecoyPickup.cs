using UnityEngine;

public class DecoyPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Decoy);
        base.Interact();
    }
}