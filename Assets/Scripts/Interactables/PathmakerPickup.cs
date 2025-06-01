using UnityEngine;

public class PathmakerPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[PathmakerPickup] Pathmaker bridge activated!");
        Destroy(gameObject);
    }
}
