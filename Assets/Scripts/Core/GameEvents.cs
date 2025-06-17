using System;

public static class GameEvents
{
    public static event Action OnShieldPickup;
    public static event Action OnTimeDilationPickup;
    public static event Action OnCloakPickup;
    public static event Action OnKeyPickup;
    public static event Action OnFireballPickup;
    public static event Action OnStormboltPickup;
    public static event Action OnBloodrootPickup;
    public static event Action OnHealthPickup;
    public static event Action OnGemCollect;
    public static event Action OnCoinCollect;
    public static event Action<int> OnTreasureChestCollect;

    public static void RaiseShieldPickup() => OnShieldPickup?.Invoke();
    public static void RaiseTimeDilationPickup() => OnTimeDilationPickup?.Invoke();
    public static void RaiseCloakPickup() => OnCloakPickup?.Invoke();
    public static void RaiseKeyPickup() => OnKeyPickup?.Invoke();
    public static void RaiseFireballPickup() => OnFireballPickup?.Invoke();
    public static void RaiseStormboltPickup() => OnStormboltPickup?.Invoke();
    public static void RaiseBloodrootPickup() => OnBloodrootPickup?.Invoke();
    public static void RaiseHealthPickup() => OnHealthPickup?.Invoke();
    public static void RaiseGemCollect() => OnGemCollect?.Invoke();
    public static void RaiseCoinCollect() => OnCoinCollect?.Invoke();
    public static void RaiseTreasureChestCollect(int coinCount) => OnTreasureChestCollect?.Invoke(coinCount);
}