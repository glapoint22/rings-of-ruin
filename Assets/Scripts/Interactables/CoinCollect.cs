public class CoinCollect : InteractableBase
{

    public override void Interact()
    {
        InteractEventManager.CollectCoin(1, CollectibleType.Coin);
        base.Interact();
    }
}