using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Items/Armor Item")]
public class ArmorItem : Item
{
    [SerializeField] private int armor;
    public int Armor => armor;
}