using UnityEngine;

public class HealthAcquire : InteractableBase
{
    [SerializeField] private int healthValue;
    private PlayerState healthAcquired;


    private void Awake()
    {
        healthAcquired = new PlayerState()
        {
            health = healthValue
        };
    }


    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(healthAcquired);
    }
}