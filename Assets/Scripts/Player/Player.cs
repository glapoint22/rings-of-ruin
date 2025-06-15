using UnityEngine;

public class Player : MonoBehaviour
{
    // Singleton instance
    public static Player Instance { get; private set; }

    // Private backing fields
    private float _health = 100f;
    private int _shieldHealth = 0;
    private bool _isCloaked = false;
    private int _armor = 0;

    private int _gems = 0;
    private int _coins = 0;

    // Public properties
    public float Health => _health;
    public int ShieldHealth => _shieldHealth;
    public bool IsCloaked => _isCloaked;
    public int Armor => _armor;
    public int Gems => _gems;
    public int Coins => _coins;


    private void Awake()
    {
        Instance = this;
    }

    // Methods to update health and shield
    public void UpdateHealth(float newHealth)
    {
        _health = newHealth;
    }

    public void UpdateShield(int newShieldHealth)
    {
        _shieldHealth = newShieldHealth;
    }

    public void UpdateGems(int newGems)
    {
        _gems = newGems;
    }

    public void UpdateCoins(int newCoins)
    {
        _coins = newCoins;
    }
}