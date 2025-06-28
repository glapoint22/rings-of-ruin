using UnityEngine;

public class Checkpoint : IPlayerState
{
    public PlayerState UpdateState(PlayerState state)
    {
        return state;
    }
}
