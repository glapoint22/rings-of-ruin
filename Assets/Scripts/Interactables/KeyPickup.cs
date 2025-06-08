using UnityEngine;

public class KeyPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Key);
        base.Interact();
    }
}