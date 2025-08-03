using UnityEngine;

public class Armor : Item
{
    [SerializeField] private int armorValue;
    public int ArmorValue => armorValue;
}
