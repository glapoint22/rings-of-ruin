using System;
using UnityEngine;

public class InteractEventManager
{
    public static event Action OnCollectGem;
    public static event Action<int, CollectibleType> OnCollectCoin;
    public static event Action<PickupType> OnPickup;

    public static void CollectGem()
    {
        OnCollectGem?.Invoke();
    }

    public static void CollectCoin(int amount, CollectibleType collectibleType)
    {
        OnCollectCoin?.Invoke(amount, collectibleType);
    }

    public static void Pickup(PickupType pickupType)
    {
        OnPickup?.Invoke(pickupType);
    }
}