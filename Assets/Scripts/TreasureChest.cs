using UnityEngine;

public class TreasureChest : IPlayerState
{
    private readonly int coinValue;

    public TreasureChest(int coinValue)
    {
        this.coinValue = coinValue;
    }
    public PlayerState UpdateState(PlayerState state)
    {
        state.coins += coinValue;
        state.hasKey = false;
        return state;
    }
}
