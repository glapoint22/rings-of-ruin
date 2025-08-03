using UnityEngine;

public abstract class RangeCombatItem : CombatItem
{
    [SerializeField] private float range;
    public float Range => range;
}