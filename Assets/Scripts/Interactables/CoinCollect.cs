public class CoinCollect : InteractableBase
{
    protected override void OnInteract()
    {
        GameEvents.RaiseCoinCollect();
    }
}