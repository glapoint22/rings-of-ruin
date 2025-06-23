using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<IPlayerState> OnCollect;
    public static event Action<PlayerState> OnCollectionUpdate;
    public static event Action<GameObject> OnInteracted;




    
    public static void RaiseCollect(IPlayerState state) => OnCollect?.Invoke(state);
    public static void RaiseCollectionUpdate(PlayerState state) => OnCollectionUpdate?.Invoke(state);
    public static void RaiseInteracted(GameObject interactable) => OnInteracted?.Invoke(interactable);
}