using UnityEngine;

public class CloakPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[CloakPickup] Cloak collected!");
        Destroy(gameObject);
    }
}