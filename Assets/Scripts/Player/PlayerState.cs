using System;

public class PlayerState {
    public int health;
    public int coins;
    public int gems;
    public int shieldHealth;
    public bool hasCloak;
    public bool hasTimeDilation;
    public bool hasKey;
    public bool hasStormbolt;
    public bool hasBloodroot;
    public bool hasFireball;
    public bool hasAshbind;
    public int armor;
    public bool isDead;

    public PlayerState()
    {
        health = 100;
        armor = 100;
    }

    public void Update(PlayerState update)
    {
        health = Math.Min(health + update.health, 100);
        coins += update.coins;
        gems += update.gems;
        shieldHealth += update.shieldHealth;
        armor += update.armor;
        hasCloak |= update.hasCloak;
        hasTimeDilation |= update.hasTimeDilation;
        hasKey |= update.hasKey;
        hasStormbolt |= update.hasStormbolt;
        hasBloodroot |= update.hasBloodroot;
        hasFireball |= update.hasFireball;
        hasAshbind |= update.hasAshbind;
    }
}