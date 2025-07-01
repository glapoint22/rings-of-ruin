using UnityEngine;

public class CoinCollect : InteractableBase
{
    [SerializeField] private int coinValue;
    private PlayerState coinCollected;

    private void Awake()
    {
        coinCollected = new PlayerState
        {
            coins = coinValue
        };
    }

    protected override void OnInteract()
    {
        GameEvents.RaiseAddCollectible(coinCollected);
        GameEvents.RaisePlayerStateUpdate(coinCollected);
    }
}