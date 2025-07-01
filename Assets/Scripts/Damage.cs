using UnityEngine;

public class Damage
{
    public DamageInfo damageInfo;


    public Damage(DamageInfo damageInfo)
    {
        this.damageInfo = damageInfo;
    }
    


    public void UpdateState(PlayerState state)
    {
        // Apply damage to shield first, then calculate armor for overflow damage
        ApplyDamageToShieldAndHealth(state, damageInfo.damage);
    }
    
    private void ApplyDamageToShieldAndHealth(PlayerState state, int rawDamage)
    {
        // Step 1: Apply damage to shield first (no armor calculation yet)
        if (state.shieldHealth > 0)
        {
            if (rawDamage >= state.shieldHealth)
            {
                // Shield is depleted, calculate overflow damage
                int overflowDamage = rawDamage - state.shieldHealth;
                state.shieldHealth = 0;
                GameEvents.RaiseRemoveBuff(BuffType.Shield);
                
                // Step 2: Apply armor calculation to overflow damage only
                if (overflowDamage > 0)
                {
                    ApplyDamageToHealth(state, overflowDamage);
                }
            }
            else
            {
                // Shield absorbs all damage (no armor calculation needed)
                state.shieldHealth -= rawDamage;
            }
        }
        else
        {
            // No shield, apply armor calculation to all damage
            ApplyDamageToHealth(state, rawDamage);
        }        
    }
    
    private void ApplyDamageToHealth(PlayerState state, int damage)
    {
        int finalDamage = CalculateDamageWithArmor(damage, state.armor);
        state.health = Mathf.Max(0, state.health - finalDamage);
    }
    
    private int CalculateDamageWithArmor(int damage, int armor)
    {
        // Apply armor calculation: Final Damage = Damage × (100 / (100 + Armor))
        if (armor > 0)
        {
            float damageMultiplier = 100f / (100f + armor);
            return Mathf.RoundToInt(damage * damageMultiplier);
        }
        
        return damage;
    }
}