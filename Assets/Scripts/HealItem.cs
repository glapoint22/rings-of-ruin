using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Items/Heal Item")]
public class HealItem : ActionItem
{
    public override void Execute()
    {
        Debug.Log("HealItem executed");
    }
}