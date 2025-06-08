using UnityEngine;
using System;

public class DamageEventManager
{
    public static event Action<DamageInfo> OnDamageDealt;
    public static event Action OnPlayerDeath;

    public static void DealDamage(DamageInfo damageInfo)
    {
        OnDamageDealt?.Invoke(damageInfo);
    }

   


    public static void PlayerDied()
    {
        Debug.Log("[DamageEventManager] Player has died.");
        OnPlayerDeath?.Invoke();
    }
}