using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<PlayerState> OnAddCollectible;
    public static event Action<GameObject> OnInteracted;
    public static event Action<PlayerState> OnPlayerStateUpdate;
    public static event Action<Damage> OnDamage;
    public static event Action<SpellType> OnAddSpell;
    public static event Action<SpellType> OnRemoveSpell;
    public static event Action<BuffType> OnAddBuff;
    public static event Action<BuffType> OnRemoveBuff;

    public static event Action OnKeyPickup;
    public static event Action<LevelData> OnLevelLoaded;
    public static event Action OnLevelCompleted;

    public static event Action<int, int> OnPlayerPlacement;

    // Targeting system events
    public static event Action<Targetable> OnTargetRegistered;
    public static event Action<Targetable> OnTargetUnregistered;
    
    // Targeting input events
    public static event Action OnTabPressed;
    public static event Action OnShiftTabPressed;
    public static event Action<Vector3> OnMouseTargetPressed;
    public static event Action OnEscapePressed;

    public static void RaiseAddCollectible(PlayerState state) => OnAddCollectible?.Invoke(state);
    public static void RaiseInteracted(GameObject interactable) => OnInteracted?.Invoke(interactable);
    public static void RaisePlayerStateUpdate(PlayerState state) => OnPlayerStateUpdate?.Invoke(state);
    public static void RaiseDamage(Damage damage) => OnDamage?.Invoke(damage);
    public static void RaiseKeyPickup() => OnKeyPickup?.Invoke();
    public static void RaiseLevelLoaded(LevelData levelData) => OnLevelLoaded?.Invoke(levelData);
    public static void RaiseLevelCompleted() => OnLevelCompleted?.Invoke();
    public static void RaisePlayerPlacement(int ringIndex, int segmentIndex) => OnPlayerPlacement?.Invoke(ringIndex, segmentIndex);
    public static void RaiseAddSpell(SpellType spell) => OnAddSpell?.Invoke(spell);
    public static void RaiseRemoveSpell(SpellType spell) => OnRemoveSpell?.Invoke(spell);
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
}