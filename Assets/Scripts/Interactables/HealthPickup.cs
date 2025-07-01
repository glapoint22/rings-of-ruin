using UnityEngine;

public class HealthPickup : InteractableBase
{
    [SerializeField] private int healthValue;
    private PlayerState healthUpdate;


    private void Awake()
    {
        healthUpdate = new PlayerState()
        {
            health = healthValue
        };
    }


    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(healthUpdate);
    }
}