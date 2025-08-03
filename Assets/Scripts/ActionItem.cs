using UnityEngine;

public abstract class ActionItem : Item
{
    [SerializeField] private float cooldown;
    public float Cooldown => cooldown;

    
    public abstract void Execute();
}