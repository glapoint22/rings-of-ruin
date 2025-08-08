using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
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
        UpdateUI();
    }


    public void AddItem(Item newItem, int amount)
    {
        item = newItem;
        quantity = amount;
        UpdateUI();
    }


    private void UpdateUI()
    {
        itemIcon.sprite = item != null ? item.Icon : null;
        quantityText.text = item != null && item.IsStackable && quantity > 1 ? quantity.ToString() : "";
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
        UpdateUI();
        return amount - amountToAdd; // Return leftover amount
    }





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
        UpdateUI();
        otherSlot.UpdateUI();
    }


    public void SetQuantity(int amount)
    {
        quantity = amount;
        UpdateUI();
    }

}