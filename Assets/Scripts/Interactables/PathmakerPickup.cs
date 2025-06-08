using UnityEngine;

public class PathmakerPickup : InteractableBase
{
    public override void Interact()
    {
        InteractEventManager.Pickup(PickupType.Pathmaker);
        base.Interact();
    }
}
