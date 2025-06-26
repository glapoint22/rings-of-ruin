public class TreasureChestCollect : InteractableBase
{
    private TreasureChest TreasureChest;
    private bool isLocked = true;

    private void Awake()
    {
        GameEvents.OnKeyPickup += OnKeyPickup;
    }

    public void SetCoinCount(int count)
    {
        TreasureChest = new TreasureChest(count);
    }

    protected override void OnInteract()
    {
        if (!isLocked) {
            GameEvents.RaiseCollect(TreasureChest);
            GameEvents.RaiseBuffExpired(PickupType.Key);
        }
    }

    protected override void RaiseInteracted()
    {
        if (!isLocked) GameEvents.RaiseInteracted(gameObject);
    }

    private void OnKeyPickup()
    {
        isLocked = false;
    }
}