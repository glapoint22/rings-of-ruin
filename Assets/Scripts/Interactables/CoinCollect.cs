using UnityEngine;

public class CoinCollect : InteractableBase
{
    [SerializeField] private int coinValue;
    private Coin Coin;
    private void Awake()
    {
        Coin = new Coin(coinValue);
    }

    protected override void OnInteract()
    {
        GameEvents.RaiseCollect(Coin);
    }
}