using UnityEngine;

public class CloakPickup : InteractableBase
{
    private readonly Cloak cloak = new();

    protected override void OnInteract()
    {
        GameEvents.RaisePickup(cloak, PickupType.Cloak);
    }
}