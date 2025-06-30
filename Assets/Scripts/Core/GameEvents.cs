using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<IPlayerState> OnCollect;
    public static event Action<PlayerState> OnCollectionUpdate;
    public static event Action<PickupType> OnPickupUpdate;
    public static event Action<GameObject> OnInteracted;
    public static event Action<IPlayerState, PortalType> OnInteract;
    public static event Action<IPlayerState, PickupType> OnPickup;
    public static event Action<Damage> OnDamage;
    public static event Action<PickupType> OnBuffExpired;

    public static event Action OnKeyPickup;
    public static event Action<LevelData> OnLevelLoaded;
    public static event Action OnLevelCompleted;

    public static event Action<int, int> OnPlayerPlacement;

    public static void RaiseCollect(IPlayerState state) => OnCollect?.Invoke(state);
    public static void RaiseCollectionUpdate(PlayerState state) => OnCollectionUpdate?.Invoke(state);
    public static void RaiseInteracted(GameObject interactable) => OnInteracted?.Invoke(interactable);
    public static void RaiseInteract(IPlayerState state, PortalType interactableType) => OnInteract?.Invoke(state, interactableType);
    public static void RaisePickup(IPlayerState state, PickupType pickupType) => OnPickup?.Invoke(state, pickupType);
    public static void RaisePickupUpdate(PickupType pickupType) => OnPickupUpdate?.Invoke(pickupType);
    public static void RaiseDamage(Damage damage) => OnDamage?.Invoke(damage);
    public static void RaiseBuffExpired(PickupType pickupType) => OnBuffExpired?.Invoke(pickupType);
    public static void RaiseKeyPickup() => OnKeyPickup?.Invoke();
    public static void RaiseLevelLoaded(LevelData levelData) => OnLevelLoaded?.Invoke(levelData);
    public static void RaiseLevelCompleted() => OnLevelCompleted?.Invoke();
    public static void RaisePlayerPlacement(int ringIndex, int segmentIndex) => OnPlayerPlacement?.Invoke(ringIndex, segmentIndex);
}