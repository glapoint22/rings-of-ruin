public class TreasureChestCollect : InteractableBase
{
    private PlayerState treasureChestUpdate;
    private bool isLocked = true;

    private void Awake()
    {
        GameEvents.OnKeyPickup += OnKeyPickup;
    }

    public void SetCoinCount(int count)
    {
        treasureChestUpdate = new PlayerState
        {
            coins = count
        };
    }

    protected override void OnInteract()
    {
        if (!isLocked) {
            GameEvents.RaiseCollect(treasureChestUpdate);
            GameEvents.RaiseRemoveBuff(BuffType.Key);
            GameEvents.RaiseCollectionUpdate(treasureChestUpdate);
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