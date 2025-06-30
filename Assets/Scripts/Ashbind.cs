using UnityEngine;

public class Ashbind : IPlayerState
{
    public PlayerState UpdateState(PlayerState state)
    {
        state.hasAshbind = true;
        return state;
    }
}