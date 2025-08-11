public static class ItemTransferState
{
    public static ItemSlot SourceSlot { get; private set; }
    public static int SplitQuantity { get; private set; }
    public static bool HasSource => SourceSlot != null;


    public static void SetSplitQuantity(int quantity) => SplitQuantity = quantity;
    
    public static void SetSource(ItemSlot slot)
    {
        SourceSlot = slot;
        SplitQuantity = 0;
    }
    
    
    
    public static void Clear()
    {
        SourceSlot = null;
        SplitQuantity = 0;
    }
}