public class Gem : IPlayerState
{
    public PlayerState UpdateState(PlayerState state)
    {
        state.gems += 1;
        return state;
    }
}