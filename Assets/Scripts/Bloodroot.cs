using UnityEngine;

public class Bloodroot : IPlayerState
{
    public PlayerState UpdateState(PlayerState state)
    {
        state.hasBloodroot = true;
        return state;
    }
}
