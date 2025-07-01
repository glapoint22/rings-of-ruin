using UnityEngine;

public class CoinCollect : InteractableBase
{
    [SerializeField] private int coinValue;
    private PlayerState coinUpdate;

    private void Awake()
    {
        coinUpdate = new PlayerState
        {
            coins = coinValue
        };
    }

    protected override void OnInteract()
    {
        GameEvents.RaiseCollect(coinUpdate);
        GameEvents.RaiseCollectionUpdate(coinUpdate);
    }
}