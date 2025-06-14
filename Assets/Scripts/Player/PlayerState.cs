using UnityEngine;
using System;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private int gemsCollected = 0;
    private int coinsCollected = 0;

    public int GemsCollected => gemsCollected;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public static event Action<int> OnGemCollected;
    public static event Action<int> OnCoinCollected;
    public static event Action<PickupType> OnBuffActivated;
    public static event Action<PickupType> OnBuffDeactivated;


    private int shieldHp = 35;
    private bool isShieldActive = false;

    private float timeDilationDuration = 10f; // Duration in seconds
    private float cloakDuration = 15f; // Duration in seconds
    private Coroutine timeDilationCoroutine;
    private Coroutine cloakCoroutine;

    private void OnEnable()
    {
        DamageEventManager.OnDamageDealt += HandleDamage;
        InteractEventManager.OnCollectGem += OnGemCollect;
        InteractEventManager.OnCollectCoin += OnCoinCollect;
        InteractEventManager.OnPickup += OnPickup;
    }

    private void OnPickup(PickupType pickupType)
    {
        switch (pickupType)
        {
            case PickupType.Health:
                OnHeal(10);
                break;
            case PickupType.Shield:
                OnShieldPickup();
                break;
            case PickupType.Key:
                OnKeyPickup();
                break;
            case PickupType.TimeDilation:
                OnTimeDilationPickup();
                break;
            case PickupType.Cloak:
                OnCloakPickup();
                break;
        }
    }


    private void Start()
    {
        currentHealth = maxHealth;
    }



    private void OnShieldPickup()
    {
        isShieldActive = true;
        Debug.Log("[PlayerState] Shield picked up");
        OnBuffActivated?.Invoke(PickupType.Shield);
    }

    private void OnKeyPickup()
    {
        Debug.Log("[PlayerState] Key picked up");
        OnBuffActivated?.Invoke(PickupType.Key);
    }

    private void OnTimeDilationPickup()
    {
        Debug.Log("[PlayerState] Time dilation picked up");
        OnBuffActivated?.Invoke(PickupType.TimeDilation);
        
        // Stop existing coroutine if it's running
        if (timeDilationCoroutine != null)
        {
            StopCoroutine(timeDilationCoroutine);
        }
        
        // Start new coroutine
        timeDilationCoroutine = StartCoroutine(TimeDilationTimer());
    }

    private void OnCloakPickup()
    {
        Debug.Log("[PlayerState] Cloak picked up");
        OnBuffActivated?.Invoke(PickupType.Cloak);
        
        // Stop existing coroutine if it's running
        if (cloakCoroutine != null)
        {
            StopCoroutine(cloakCoroutine);
        }
        
        // Start new coroutine
        cloakCoroutine = StartCoroutine(CloakTimer());
    }

    private IEnumerator TimeDilationTimer()
    {
        yield return new WaitForSeconds(timeDilationDuration);
        OnBuffDeactivated?.Invoke(PickupType.TimeDilation);
        Debug.Log("[PlayerState] Time dilation ended");
    }

    private IEnumerator CloakTimer()
    {
        yield return new WaitForSeconds(cloakDuration);
        OnBuffDeactivated?.Invoke(PickupType.Cloak);
        Debug.Log("[PlayerState] Cloak ended");
    }

    private void OnGemCollect()
    {
        gemsCollected++;
        OnGemCollected?.Invoke(gemsCollected);
    }

    private void OnCoinCollect(int amount, CollectibleType collectibleType)
    {
        coinsCollected += amount;
        OnCoinCollected?.Invoke(coinsCollected);

        if (collectibleType == CollectibleType.TreasureChest)
        {
            OnBuffDeactivated?.Invoke(PickupType.Key);
            
        }
    }

    private void OnHeal(int amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        Debug.Log($"[PlayerState] Health: {currentHealth}/{maxHealth}");
    }

    

   

    public void ResetLevelStats()
    {
        gemsCollected = 0;
        currentHealth = maxHealth; // Reset health when starting a new level
        Debug.Log($"[PlayerState] Health reset to: {currentHealth}/{maxHealth}");
        // Reset other temporary states like shields, cloak, spell, etc. as we add them
    }




    public void HandleDamage(DamageInfo damageInfo)
    {
        if (damageInfo.Amount > 0)
        {
            if (isShieldActive)
            {
                int remainingDamage = OnShieldHit(damageInfo.Amount);
                if (remainingDamage > 0)
                {
                    // Apply remaining damage to player
                    currentHealth = Mathf.Max(0, currentHealth - remainingDamage);
                    Debug.Log($"[PlayerState] currentHealth: {currentHealth}");

                }
                return;
            }

            Debug.Log("Damage Taken");
            currentHealth = Mathf.Max(0, currentHealth - damageInfo.Amount);

        }

        if (currentHealth <= 0 || damageInfo.DamageType == DamageType.Fall)
        {
            DamageEventManager.PlayerDied();
        }
    }

    public int OnShieldHit(int amount)
    {
        int remainingDamage = 0;

        if (amount > shieldHp)
        {
            // Calculate remaining damage that will spill over
            remainingDamage = amount - shieldHp;
            shieldHp = 0;
        }
        else
        {
            shieldHp -= amount;
        }

        Debug.Log($"[PlayerState] Shield HP: {shieldHp}");

        if (shieldHp <= 0)
        {
            isShieldActive = false;
            OnBuffDeactivated?.Invoke(PickupType.Shield);
        }

        return remainingDamage;
    }

    private void OnDisable()
    {
        DamageEventManager.OnDamageDealt -= HandleDamage;
        InteractEventManager.OnCollectGem -= OnGemCollect;
        InteractEventManager.OnCollectCoin -= OnCoinCollect;
        InteractEventManager.OnPickup -= OnPickup;
    }
}