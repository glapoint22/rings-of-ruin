using UnityEngine;

public class Stormbolt : IPlayerState
{
    public PlayerState UpdateState(PlayerState state)
    {
        state.hasStormbolt = true;
        return state;
    }
}
