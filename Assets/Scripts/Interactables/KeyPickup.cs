using UnityEngine;

public class KeyPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[KeyPickup] Key collected!");
        Destroy(gameObject);
    }
}