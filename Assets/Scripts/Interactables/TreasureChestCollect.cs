public class TreasureChestCollect : InteractableBase
{
    private int coinCount; 

    public void SetCoinCount(int count)
    {
        coinCount = count;
    }

    public override void Interact()
    {
        InteractEventManager.CollectCoin(coinCount, CollectibleType.TreasureChest);
        base.Interact();
    }
}