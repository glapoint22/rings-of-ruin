public class Fireball : IPlayerState
{
    public PlayerState UpdateState(PlayerState state)
    {
        state.hasFireball = true;
        return state;
    }
}