public class Coin : IPlayerState
{
    private readonly int coinValue;

    public Coin(int coinValue) {
        this.coinValue = coinValue;
    }

    public PlayerState UpdateState(PlayerState state)
    {
        state.coins += coinValue;
        return state;
    }
}