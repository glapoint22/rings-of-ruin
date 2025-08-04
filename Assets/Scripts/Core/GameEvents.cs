using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<PlayerState> OnPlayerStateUpdate;
    public static event Action<Damage> OnDamage;
    public static event Action<BuffType> OnAddBuff;
    public static event Action<BuffType> OnRemoveBuff;
    public static event Action<int> OnItemCollectionClick;



    // Targeting system events
    public static event Action<Targetable> OnTargetRegistered;
    public static event Action<Targetable> OnTargetUnregistered;
    
    // Targeting input events
    public static event Action OnTabPressed;
    public static event Action OnShiftTabPressed;
    public static event Action<Vector3> OnMouseTargetPressed;
    public static event Action OnEscapePressed;
    public static event Action<Vector3> OnRightMousePressed;
    public static event Action OnShiftPressed;

    // Path completion events

    public static void RaisePlayerStateUpdate(PlayerState state) => OnPlayerStateUpdate?.Invoke(state);
    public static void RaiseDamage(Damage damage) => OnDamage?.Invoke(damage);
    public static void RaiseAddBuff(BuffType buff) => OnAddBuff?.Invoke(buff);
    public static void RaiseRemoveBuff(BuffType buff) => OnRemoveBuff?.Invoke(buff);

    // Targeting system raise methods
    public static void RaiseTargetRegistered(Targetable target) => OnTargetRegistered?.Invoke(target);
    public static void RaiseTargetUnregistered(Targetable target) => OnTargetUnregistered?.Invoke(target);
    
    // Targeting input raise methods
    public static void RaiseTabPressed() => OnTabPressed?.Invoke();
    public static void RaiseShiftTabPressed() => OnShiftTabPressed?.Invoke();
    public static void RaiseMouseTargetPressed(Vector3 worldPosition) => OnMouseTargetPressed?.Invoke(worldPosition);
    public static void RaiseEscapePressed() => OnEscapePressed?.Invoke();
    public static void RaiseRightMousePressed(Vector3 worldPosition) => OnRightMousePressed?.Invoke(worldPosition);

    // Item collection raise methods
    public static void RaiseItemCollectionClick(int quantity) => OnItemCollectionClick?.Invoke(quantity);
    public static void RaiseShiftPressed() => OnShiftPressed?.Invoke();
}