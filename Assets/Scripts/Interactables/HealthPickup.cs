using UnityEngine;

public class HealthPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Health);
        base.Interact();
    }
}
