using UnityEngine;

public class TimeDilationPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[TimeDilationPickup] Time dilation activated!");
        Destroy(gameObject);
    }
}
