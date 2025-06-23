public class TreasureChestCollect : InteractableBase
{
    private Coin Coin;

    public void SetCoinCount(int count)
    {

        Coin = new Coin(count);
    }

    protected override void OnInteract()
    {
        GameEvents.RaiseCollect(Coin);
    }
}