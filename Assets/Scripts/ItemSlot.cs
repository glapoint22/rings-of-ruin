using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot: MonoBehaviour
{

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;

    private Item item;
    private int quantity;

    public bool IsEmpty => item == null;
    public Item Item => item;
    public int Quantity => quantity;

    public void ClearSlot()
    {
        item = null;
        quantity = 0;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        quantityText.text = "";
    }


    public void AddItem(Item newItem, int amount)
    {
        item = newItem;
        quantity = item.IsStackable
            ? Mathf.Clamp(amount, 1, item.MaxStackSize)
            : 1;
        itemIcon.sprite = item.Icon;
        itemIcon.enabled = true;
        quantityText.text =  item.IsStackable ? quantity.ToString() : "";
    }

    public bool CanStackWith(Item otherItem)
    {
        return !IsEmpty &&
               item == otherItem && // Same item type
               item.IsStackable &&
               quantity < item.MaxStackSize;
    }


    public int AddToStack(int amount)
    {
        int spaceLeft = item.MaxStackSize - quantity;
        int amountToAdd = Mathf.Min(spaceLeft, amount);
        quantity += amountToAdd;
        return amount - amountToAdd; // Return leftover amount
    }


    // public int TryAddItem(Item newItem, int amount)
    // {
    //     if (IsEmpty)
    //     {
    //         AddItem(newItem, amount);
    //         return 0; // No leftover
    //     }
    //     else if (CanStackWith(newItem))
    //     {
    //         return AddToStack(amount);
    //     }

    //     return amount; // Couldn't add anything
    // }



    public void RemoveFromStack(int amount)
    {
        quantity -= amount;
        if (quantity <= 0) ClearSlot();
    }

    public void SwapItem(ItemSlot otherSlot)
    {
        Item tempItem = item;
        int tempQuantity = quantity;
        item = otherSlot.item;
        quantity = otherSlot.quantity;
        otherSlot.item = tempItem;
        otherSlot.quantity = tempQuantity;
    }
}