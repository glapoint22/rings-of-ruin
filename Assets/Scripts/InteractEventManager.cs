using System;
using UnityEngine;

public class InteractEventManager
{
    public static event Action<CollectibleType> OnCollect;
    public static event Action<PickupType> OnPickup;

    public static void Collect(CollectibleType collectibleType)
    {
        OnCollect?.Invoke(collectibleType);
    }


    public static void Pickup(PickupType pickupType)
    {
        OnPickup?.Invoke(pickupType);
    }
}