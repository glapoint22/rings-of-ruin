using UnityEngine;

public class DecoyPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[DecoyPickup] Decoy clone triggered!");
        Destroy(gameObject);
    }
}