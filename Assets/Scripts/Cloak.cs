using UnityEngine;

public class Cloak : IPlayerState, ITimeBasedBuff
{
    public float Duration { get; } = 10f;
    public PlayerState UpdateState(PlayerState state)
    {
        state.hasCloak = true;
        return state;
    }

    public PlayerState OnBuffExpired(PlayerState state)
    {
        state.hasCloak = false;
        GameEvents.RaiseBuffExpired(PickupType.Cloak);
        return state;
    }
}