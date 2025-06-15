using UnityEngine;

public class CoinCollect : InteractableBase
{
    [SerializeField] private int coinValue = 1;

    public override void Interact()
    {
        Collect(CollectibleType.Coin, coinValue);
        base.Interact();
    }
}