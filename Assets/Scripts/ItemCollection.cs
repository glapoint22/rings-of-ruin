using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] private SplitStackForm splitStackForm;


    private bool isShiftPressed = false;
    private int splitQuantity = 0;
    private ItemSlot sourceSlot;





    private void OnEnable()
    {
        GameEvents.OnShiftPressed += HandleShiftPressed;
        GameEvents.OnStackSplit += HandleStackSplit;
        GameEvents.OnShiftReleased += HandleShiftReleased;
        GameEvents.OnStackSplitCancel += HandleStackSplitCancel;
    }

    private void OnDisable()
    {
        GameEvents.OnShiftPressed -= HandleShiftPressed;
        GameEvents.OnStackSplit -= HandleStackSplit;
        GameEvents.OnShiftReleased -= HandleShiftReleased;
        GameEvents.OnStackSplitCancel -= HandleStackSplitCancel;
    }

    private void HandleShiftPressed() => isShiftPressed = true;
    private void HandleShiftReleased() => isShiftPressed = false;
    private void HandleStackSplit(int quantity) => splitQuantity = quantity;
    private void HandleStackSplitCancel()
    {
        splitQuantity = 0;
        sourceSlot = null;
    }







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





    public void OnDrag(ItemSlot slot)
    {
        if (isShiftPressed)
        {
            HandleShiftPressed(slot);
            return;
        }

        if (splitStackForm.gameObject.activeSelf) CloseSplitForm();

        SetSourceSlot(slot);
    }


    private void SetSourceSlot(ItemSlot slot)
    {
        if (slot.IsEmpty) return;
        sourceSlot = slot;
    }




    public void OnDrop(ItemSlot slot)
    {
        if (sourceSlot == null || splitStackForm.gameObject.activeSelf) return;


        SetSlot(slot, sourceSlot.Quantity);
        sourceSlot = null;
    }

    private void OpenSplitForm(ItemSlot slot)
    {
        splitStackForm.gameObject.SetActive(true);
        splitStackForm.SetQuantity(slot.Quantity);
    }


    private void CloseSplitForm()
    {
        splitStackForm.gameObject.SetActive(false);
        sourceSlot = null;
    }


    public void OnClick(ItemSlot slot)
    {
        if (isShiftPressed)
        {
            HandleShiftPressed(slot);
            return;
        }

        if (splitStackForm.gameObject.activeSelf) CloseSplitForm();


        if (sourceSlot == null)
        {
            SetSourceSlot(slot);
            return;
        }

        if (splitQuantity > 0)
        {
            SplitItems(slot);
            return;
        }

        SetSlot(slot, sourceSlot.Quantity);
        sourceSlot = null;
    }


    private void HandleShiftPressed(ItemSlot slot)
    {
        if (!slot.IsEmpty && slot.Item.IsStackable && slot.Quantity > 1 && sourceSlot == null)
        {
            OpenSplitForm(slot);
            SetSourceSlot(slot);
        }
    }



    private void SplitItems(ItemSlot slot)
    {
        int diff = sourceSlot.Quantity - splitQuantity;
        SetSlot(slot, splitQuantity, diff == 0);
        if (diff > 0) sourceSlot.SetQuantity(diff);
        sourceSlot = null;
        splitQuantity = 0;
    }



    private void SetSlot(ItemSlot slot, int quantity, bool clearSource = true)
    {

        if (slot == sourceSlot) return;

        if (slot.IsEmpty)
        {
            slot.AddItem(sourceSlot.Item, quantity);
            if (clearSource) sourceSlot.ClearSlot();
            return;
        }


        if (slot.CanStackWith(sourceSlot.Item) && quantity < slot.Item.MaxStackSize)
        {
            StackItems(slot, quantity, clearSource);
            return;
        }

        sourceSlot.SwapItem(slot);
    }


    private void StackItems(ItemSlot slot, int quantity, bool clearSource)
    {
        int leftover = slot.AddToStack(quantity);

        if (leftover == 0 && clearSource)
        {
            sourceSlot.ClearSlot();
            return;
        }

        sourceSlot.SetQuantity(leftover);
    }
}