using UnityEngine;

public class HealthPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[HealthPickup] Health restored!");
        Destroy(gameObject);
    }
}
