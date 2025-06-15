using UnityEngine;
using System;

public class InteractableBase : MonoBehaviour
{
    public static event Action<CollectibleType, int> OnCollect;
    public static event Action<PickupType> OnPickup;


    private void OnTriggerEnter(Collider other)
    {
        Interact();
    }

    public virtual void Interact()
    {
        Destroy(gameObject);
    }

    protected void Collect(CollectibleType collectibleType, int count = 1)
    {
        OnCollect?.Invoke(collectibleType, count);
    }

    protected void Pickup(PickupType pickupType)
    {
        OnPickup?.Invoke(pickupType);
    }
}