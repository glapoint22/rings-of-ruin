using UnityEngine;

public abstract class CombatItem : ActionItem
{
    [SerializeField] private int damage;
    public int Damage => damage;
}