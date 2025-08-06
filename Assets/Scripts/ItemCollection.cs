using UnityEngine;

public class ItemCollection : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] private SpitterForm splitForm;


    private bool isShiftPressed = false;
    private int splitQuantity = 0;
    private ItemSlot sourceSlot;





    private void OnEnable()
    {
        GameEvents.OnShiftPressed += () => isShiftPressed = true;
        GameEvents.OnItemCollectionClick += (quantity) => splitQuantity = quantity;
        GameEvents.OnShiftReleased += () => isShiftPressed = false;
    }







    public void AddItem(Item item, int amount = 1)
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





    public void OnDrag(ItemSlot slot)
    {
        if (splitForm.gameObject.activeSelf) CloseSplitForm();

        SetSourceSlot(slot);
    }


    private void SetSourceSlot(ItemSlot slot)
    {
        if (slot.IsEmpty) return;
        sourceSlot = slot;
    }




    public void OnDrop(ItemSlot slot)
    {
        if (sourceSlot == null) return;

        if (isShiftPressed && sourceSlot.Item.IsStackable)
        {
            OpenSplitForm(sourceSlot);
            return;
        }

        SetSlot(slot, sourceSlot.Quantity);
    }

    private void OpenSplitForm(ItemSlot slot)
    {
        splitForm.gameObject.SetActive(true);
        splitForm.SetQuantity(slot.Quantity);
        isShiftPressed = false;
    }


    private void CloseSplitForm()
    {
        splitForm.gameObject.SetActive(false);
        sourceSlot = null;
    }


    public void OnClick(ItemSlot slot)
    {
        if (slot == sourceSlot)
        {
            sourceSlot = null;
            splitQuantity = 0;
            CloseSplitForm();
            return;
        }

        if (splitForm.gameObject.activeSelf) CloseSplitForm();

        if (isShiftPressed && slot.Item.IsStackable && sourceSlot == null) OpenSplitForm(slot);

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
    }



    private void SplitItems(ItemSlot slot)
    {
        int diff = sourceSlot.Quantity - splitQuantity;
        SetSlot(slot, splitQuantity, diff == 0);
        if (diff > 0) sourceSlot.SetQuantity(diff);

        splitQuantity = 0;
        sourceSlot = null;
    }



    private void SetSlot(ItemSlot slot, int quantity, bool clearSource = true)
    {
        if (slot.IsEmpty)
        {
            slot.AddItem(sourceSlot.Item, quantity);
            if (clearSource) ClearSourceSlot();
        }
        else
        {
            if (slot.CanStackWith(sourceSlot.Item) && quantity < slot.Item.MaxStackSize)
            {
                StackItems(slot, quantity, clearSource);
            }
            else
            {
                SwapItems(slot);
            }
        }
    }


    private void StackItems(ItemSlot slot, int quantity, bool clearSource)
    {
        int leftover = slot.AddToStack(quantity);

        if (leftover == 0 && clearSource)
        {
            ClearSourceSlot();
            return;
        }

        sourceSlot.SetQuantity(leftover);
    }


    private void SwapItems(ItemSlot slot)
    {
        sourceSlot.SwapItem(slot);
        sourceSlot = null;
    }





    private void ClearSourceSlot()
    {
        sourceSlot.ClearSlot();
        sourceSlot = null;
    }
}