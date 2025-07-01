public class TreasureChestCollect : InteractableBase
{
    private PlayerState treasureChestCollected;
    private bool isLocked = true;

    private void Awake()
    {
        GameEvents.OnKeyPickup += OnKeyPickup;
    }

    public void SetCoinCount(int count)
    {
        treasureChestCollected = new PlayerState
        {
            coins = count
        };
    }

    protected override void OnInteract()
    {
        if (!isLocked) {
            GameEvents.RaiseRemoveBuff(BuffType.Key);
            GameEvents.RaiseAddCollectible(treasureChestCollected);
            GameEvents.RaisePlayerStateUpdate(treasureChestCollected);
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