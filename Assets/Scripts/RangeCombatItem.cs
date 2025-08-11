using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Items/Range Combat Item")]
public class RangeCombatItem : CombatItem
{
    [SerializeField] private float range;
    public float Range => range;

    public override void Execute()
    {
        Debug.Log("RangeCombatItem executed");
    }
}