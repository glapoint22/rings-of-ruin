using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;
    [SerializeField] private int value;
    [SerializeField] private int maxStackSize = 1;
    [SerializeField] private ItemType itemType;

    public string ItemName => itemName;
    public Sprite Icon => icon;
    public string Description => description;
    public int Value => value;
    public int MaxStackSize => maxStackSize;
    public bool IsStackable => maxStackSize > 1;
}