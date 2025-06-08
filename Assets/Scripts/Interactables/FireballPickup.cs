using UnityEngine;

public class FireballPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Fireball);
        base.Interact();
    }
} 