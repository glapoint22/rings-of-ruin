using UnityEngine;

public class Key : IPlayerState
{
    public PlayerState UpdateState(PlayerState state)
    {
        state.hasKey = true;
        return state;
    }
}