using UnityEngine;

public class ShieldPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[ShieldPickup] Shield collected!");
        // TODO: Add shield to player when shield system is ready
        Destroy(gameObject);
    }
}
