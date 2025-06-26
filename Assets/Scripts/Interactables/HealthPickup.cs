using UnityEngine;

public class HealthPickup : InteractableBase
{
    [SerializeField] private int healthValue;
    private Health Health;
    private void Awake()
    {
        Health = new Health(healthValue);
    }

    protected override void OnInteract()
    {
        GameEvents.RaisePickup(Health, PickupType.Health);
    }
}