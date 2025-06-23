// using UnityEngine;

// public class DamageManager
// {
//     // Armor effectiveness constant - 1 point of armor reduces damage by 1%
//     private const float ARMOR_EFFECTIVENESS = 0.01f;
//     private const float MAX_DAMAGE_REDUCTION = 0.75f; // Maximum 75% damage reduction
    

//     public static void UpdateDamage(int rawDamage)
//     {
//         int finalDamage = CalculateFinalDamage(rawDamage);
//         ApplyDamageToPlayer(finalDamage);
//     }

//     private static int CalculateFinalDamage(int rawDamage)
//     {
//         int damageAfterShield = CalculateDamageAfterShield(rawDamage);
//         return Player.Instance.Armor > 0 && damageAfterShield > 0
//             ? CalculateDamageAfterArmor(damageAfterShield)
//             : damageAfterShield;
//     }



//     private static void ApplyDamageToPlayer(int damage)
//     {
//         float newHealth = Mathf.Max(0, Player.Instance.Health - damage);
//         Player.Instance.UpdateHealth(newHealth);
//     }

    

//     private static int CalculateDamageAfterShield(int amount)
//     {
//         if (Player.Instance.ShieldHealth <= 0) return amount;

//         int remainingDamage = 0;
//         if (amount > Player.Instance.ShieldHealth) {
//             remainingDamage = amount - Player.Instance.ShieldHealth;
//             Player.Instance.UpdateShield(0);
//         } else {
//             int newShieldHealth = Mathf.Max(0, Player.Instance.ShieldHealth - amount);
//             Player.Instance.UpdateShield(newShieldHealth);
//         }
//         return remainingDamage;
//     }

//     private static int CalculateDamageAfterArmor(int amount)
//     {
//         float damageReduction = Mathf.Min(Player.Instance.Armor * ARMOR_EFFECTIVENESS, MAX_DAMAGE_REDUCTION);
//         return Mathf.RoundToInt(amount * (1 - damageReduction));
//     }
// }