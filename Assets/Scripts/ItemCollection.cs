using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] private SplitStackForm splitStackForm;
    [SerializeField] private GameObject modelBackground;

    private bool isShiftPressed = false;

    private void OnEnable()
    {
        GameEvents.OnShiftPressed += HandleShiftPressed;
        GameEvents.OnShiftReleased += HandleShiftReleased;
    }

    private void OnDisable()
    {
        GameEvents.OnShiftPressed -= HandleShiftPressed;
        GameEvents.OnShiftReleased -= HandleShiftReleased;
    }

    private void HandleShiftPressed() => isShiftPressed = true;
    private void HandleShiftReleased() => isShiftPressed = false;
    

    public void AddItem(Item item, int amount = 1)
    {
        if (item.IsStackable)
        {
            AddStackableItem(item, amount);
            return;
        }

        AddNonStackableItem(item, amount);
    }

    private void AddStackableItem(Item item, int amount)
    {
        FindFirstStackableAndEmpty(item, out var stackable, out var empty);

        if (stackable != null)
        {
            int leftover = stackable.AddToStack(amount);
            if (leftover == 0) return;

            if (empty != null)
            {
                empty.AddItem(item, leftover);
                return;
            }

            LogInventoryFull();
            return;
        }

        if (empty != null)
        {
            empty.AddItem(item, amount);
            return;
        }

        LogInventoryFull();
    }

    private void AddNonStackableItem(Item item, int amount)
    {
        var empty = FindFirstEmptySlot();
        if (empty != null)
        {
            empty.AddItem(item, amount);
            return;
        }

        LogInventoryFull();
    }

    private void FindFirstStackableAndEmpty(Item item, out ItemSlot stackable, out ItemSlot empty)
    {
        stackable = null;
        empty = null;

        for (int i = 0; i < itemSlots.Length && (stackable == null || empty == null); i++)
        {
            var slot = itemSlots[i];

            if (stackable == null && slot.CanStackWith(item))
                stackable = slot;

            if (empty == null && slot.IsEmpty)
                empty = slot;
        }
    }

    private ItemSlot FindFirstEmptySlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].IsEmpty) return itemSlots[i];
        }
        return null;
    }

    private void LogInventoryFull()
    {
        Debug.Log("No space left in the inventory");
    }

    public void OnBeginDrag(ItemSlot slot)
    {
        if (isShiftPressed)
        {
            HandleShiftPressed(slot);
            return;
        }


        SetSourceSlot(slot);
    }

    private void SetSourceSlot(ItemSlot slot)
    {
        if (slot.IsEmpty) return;
        ItemTransferState.SetSource(slot);
    }

    public void OnDrop(ItemSlot slot)
    {
        if (!ItemTransferState.HasSource) return;

        SetSlot(slot, ItemTransferState.SourceSlot.Quantity);
        ItemTransferState.Clear();
    }

    private void OpenSplitForm(ItemSlot slot)
    {
        modelBackground.SetActive(true);
        splitStackForm.SetQuantity(slot.Quantity);
    }


    public void OnClick(ItemSlot slot)
    {
        if (isShiftPressed)
        {
            HandleShiftPressed(slot);
            return;
        }


        if (!ItemTransferState.HasSource)
        {
            SetSourceSlot(slot);
            return;
        }

        if (ItemTransferState.SplitQuantity > 0)
        {
            SplitItems(slot);
            return;
        }

        SetSlot(slot, ItemTransferState.SourceSlot.Quantity);
        ItemTransferState.Clear();
    }

    private void HandleShiftPressed(ItemSlot slot)
    {
        if (!slot.IsEmpty && slot.Item.IsStackable && slot.Quantity > 1 && !ItemTransferState.HasSource)
        {
            OpenSplitForm(slot);
            SetSourceSlot(slot);
        }
    }

    private void SplitItems(ItemSlot slot)
    {
        int diff = ItemTransferState.SourceSlot.Quantity - ItemTransferState.SplitQuantity;
        SetSlot(slot, ItemTransferState.SplitQuantity, diff == 0);
        if (diff > 0) ItemTransferState.SourceSlot.SetQuantity(diff);
        ItemTransferState.Clear();
    }

    private void SetSlot(ItemSlot slot, int quantity, bool clearSource = true)
    {
        if (slot == ItemTransferState.SourceSlot) return;

        if (slot.IsEmpty)
        {
            slot.AddItem(ItemTransferState.SourceSlot.Item, quantity);
            if (clearSource) ItemTransferState.SourceSlot.ClearSlot();
            return;
        }

        if (slot.CanStackWith(ItemTransferState.SourceSlot.Item) && quantity < slot.Item.MaxStackSize)
        {
            StackItems(slot, quantity, clearSource);
            return;
        }

        ItemTransferState.SourceSlot.SwapItem(slot);
    }

    private void StackItems(ItemSlot slot, int quantity, bool clearSource)
    {
        int leftover = slot.AddToStack(quantity);

        if (leftover == 0 && clearSource)
        {
            ItemTransferState.SourceSlot.ClearSlot();
            return;
        }

        ItemTransferState.SourceSlot.SetQuantity(leftover);
    }
}