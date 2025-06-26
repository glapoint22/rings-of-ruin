using UnityEngine;

public class CloakPickup : InteractableBase
{
    [SerializeField] private float duration;
    private Cloak cloak;
    private void Awake()
    {
        cloak = new Cloak(duration);
    }

    protected override void OnInteract()
    {
        GameEvents.RaisePickup(cloak, PickupType.Cloak);
    }
}