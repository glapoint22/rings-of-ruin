using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;

    public void AddItem(Item item, int amount)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item, amount);
                return;
            }
            else if (slot.CanStackWith(item))
            {
                int leftover = slot.AddToStack(amount);
                if (leftover == 0)
                {
                    return;
                }
                amount = leftover;
            }
        }
        Debug.Log("No space left in the inventory");
    }

}
