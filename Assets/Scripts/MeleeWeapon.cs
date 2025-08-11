using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Items/Melee Weapon")]
public class MeleeWeapon : CombatItem
{
    public override void Execute()
    {
        Debug.Log("MeleeWeapon executed");
    }
}