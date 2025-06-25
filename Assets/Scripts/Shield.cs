public class Shield : IPlayerState
{
    public int shieldHealth;
    public Shield(int shieldHealth)
    {
        this.shieldHealth = shieldHealth;
    }
    public PlayerState UpdateState(PlayerState state)
    {
        state.shieldHealth = shieldHealth;
        return state;
    }
}