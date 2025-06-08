public class CoinCollect : InteractableBase
{

    public override void Interact()
    {
        InteractEventManager.Collect(CollectibleType.Coin);
        base.Interact();
    }
}